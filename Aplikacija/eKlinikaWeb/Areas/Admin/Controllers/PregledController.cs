using eKlinika.DataAccess.Data;
using eKlinika.DataAccess.Repository.IRepository;
using eKlinika.Models;
using eKlinika.Models.ViewModels;
using eKlinika.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Security.Claims;

namespace eKlinikaWeb.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class PregledController : Controller
    {
        private readonly IUnitOfWork UnitOfWork;

        private readonly IEmailSender _emailSender;

        [BindProperty]
        public PregledVM pregledVM { get; set; }


        public PregledController(IUnitOfWork _unitofwork)
        {
            UnitOfWork = _unitofwork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            pregledVM = new PregledVM()
            {
                listaOdeljenja = UnitOfWork.Odeljenja.GetAll()
                .OrderBy(x => x.NazivOdeljenja)
                .Select(x => new SelectListItem
                {
                    Value = x.OdeljenjeID.ToString(),
                    Text = x.NazivOdeljenja
                }).ToList(),
                listaVremena = UnitOfWork.Vreme.GetAll()
                .OrderBy(x => x.VremeTimeSpan)
                .Select(x => new SelectListItem
                {
                    Value = x.VremeID.ToString(),
                    Text = x.VremeTimeSpan.ToString().TrimEnd('0').TrimEnd(':')
                }),
                Pregled = new()
            };
            pregledVM.Pregled.ApplicationUser = UnitOfWork.ApplicationUsers.GetFirstOrDefault(x => x.Id == claim.Value);
            pregledVM.Pregled.Ime = pregledVM.Pregled.ApplicationUser.Ime;
            if(pregledVM.Pregled.ApplicationUser.BrojTelefona != null)
                pregledVM.Pregled.Telefon = pregledVM.Pregled.ApplicationUser.BrojTelefona;

            //List<string> query = Enumerable.Range(0, 33).Select(i =>
            //DateTime.Today.AddHours(9).AddMinutes(i * 15).ToString()).ToList();

            //var listaVremena = query.Select(x => new SelectListItem
            //{
            //    Value = x,
            //    Text = x
            //}).ToList();
            //pregledVM.listaVremena = listaVremena;
            return View(pregledVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Zakazi")]

        public IActionResult Zakazi(PregledVM pregledVM)
        {
            ModelState.Remove("Pregled.ApplicationUserID");
            if (ModelState.IsValid)
            {
                //Proveriti da li je zakazno vreme manje od trenutnog vremena. Ako da onda prikazati gresku
                if (Convert.ToDateTime(pregledVM.Pregled.Datum) < System.DateTime.Now.Date)
                {
                    //Ovo je vec reseno s frontEnd strane ali ovde je reseno i s backend.
                    TempData["error"] = "Greska";
                    return RedirectToAction("Index");
                }
                else
                {
                    //Onda je to ili sadasnji datum ili neki dan u buducnosti

                    //Ako je sadanji datum, moramo proveriti da li je zakazano vreme manje u odnosu na trenutno vreme
                    //Ako jeste izbaciti gresku
                    TimeSpan pregledVMVreme = UnitOfWork.Vreme.GetFirstOrDefault(x => x.VremeID == pregledVM.Pregled.VremeID).VremeTimeSpan;
                    string trenutnoVreme = DateTime.Now.ToString("HH:mm:ss");
                    TimeSpan trenutnoVremeTimeSpan = TimeSpan.Parse(trenutnoVreme);
                    if (TimeSpan.Compare(pregledVMVreme, trenutnoVremeTimeSpan) < 0)
                    {
                        TempData["error"] = "Termin u koji ste zakazali je prosao.";
                        return RedirectToAction("Index");
                    }
                    //Pre nego sto dodamo pregled. Moramo proveriti da li vec postoji takav tip pregleda tog datuma.
                    //Moramo isto proveriti da li vec postoji neki drugi pregled u to vreme. Ako su ovi uslovi ispunjeni izbaciti gresku
                    var claimsIdentity = (ClaimsIdentity)User.Identity;
                    var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                    Odeljenje odeljenjeFromDB = UnitOfWork.Odeljenja.GetFirstOrDefault(x => x.OdeljenjeID == pregledVM.Pregled.OdeljenjeID);
                    
                    Pregled pregledFromDBZaTajDatum = UnitOfWork.Pregledi.GetFirstOrDefault(x => x.ApplicationUserID == claim.Value && x.Datum == pregledVM.Pregled.Datum && x.Odeljenje.NazivOdeljenja == odeljenjeFromDB.NazivOdeljenja);
                    if (pregledFromDBZaTajDatum != null)
                    {
                        TempData["error"] = "Vec ste zakazali taj pregled za izabrani dan.";
                        return RedirectToAction("Index");
                    }
                    Pregled pregledFromDBZaTajPregledZaToVreme = UnitOfWork.Pregledi.GetFirstOrDefault(x => x.ApplicationUserID == claim.Value && x.Datum == pregledVM.Pregled.Datum && x.VremeID == pregledVM.Pregled.VremeID);
                    if (pregledFromDBZaTajPregledZaToVreme != null)
                    {
                        TempData["error"] = "Imate drugi pregled zakazano u to vreme.";
                        return RedirectToAction("Index");
                    }

                    //Nakon ovih provera mozemo dodati pregled
                    pregledVM.Pregled.ApplicationUser = UnitOfWork.ApplicationUsers.GetFirstOrDefault(x => x.Id == claim.Value);
                    pregledVM.Pregled.Ime = pregledVM.Pregled.ApplicationUser.Ime;
                    pregledVM.Pregled.Telefon = pregledVM.Pregled.ApplicationUser.BrojTelefona;

                    pregledVM.Pregled.ApplicationUserID = claim.Value;
                    UnitOfWork.Pregledi.Add(pregledVM.Pregled);
                    UnitOfWork.Save();
                    TempData["success"] = "Uspesno napravljen pregled";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["error"] = "Neki podaci su lose uneti";
                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult PregledIndex()
        {
            return View();
        }
        //GET
        public IActionResult Izmeni(int? id)
        {
            Pregled p = UnitOfWork.Pregledi.GetFirstOrDefault(x => x.PregledID == id);
            Odeljenje OdeljenjeFromDB = UnitOfWork.Odeljenja.GetFirstOrDefault(x => x.OdeljenjeID == p.OdeljenjeID);
            p.Odeljenje = OdeljenjeFromDB;
            return View(p);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var productList = UnitOfWork.Pregledi.GetAll(x => x.ApplicationUserID == claim.Value,includeProperties:"Odeljenje");
            return Json(new { data = productList });
        }
        //POST
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = UnitOfWork.Pregledi.GetFirstOrDefault(u => u.PregledID == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Greska prilikom brisanja" });
            }
            UnitOfWork.Pregledi.Remove(obj);
            UnitOfWork.Save();
            return Json(new { success = true, message = "Uspesno obrisano" });
        }
        #endregion
    }
}

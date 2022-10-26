using eKlinika.DataAccess.Data;
using eKlinika.DataAccess.Repository.IRepository;
using eKlinika.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
namespace eKlinikaWeb.Controllers
{
    [Area("Admin")]
    public class PacijentController : Controller
    {
        private readonly IUnitOfWork UnitOfWork;

        private bool hasBigLetter(string password)
        {
            for (int i = 0; i < password.Length; i++)
            {
                if (char.IsUpper(password[i]))
                    return true;
                else
                {
                    continue;
                }
            }
            return false;
        }
        private bool hasDigit(string password)
        {
            for (int i = 0; i < password.Length; i++)
            {
                if (char.IsDigit(password[i]))
                    return true;
                else
                {
                    continue;
                }
            }
            return false;
        }
        private bool hasRequirements(string password)
        {
            if (String.IsNullOrEmpty(password))
                return false;
            var item = new Regex(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$");
            if (item.IsMatch(password))
            {
                return true;
            }
            return false;
        }
        public PacijentController(IUnitOfWork _unitofwork)
        {
            UnitOfWork = _unitofwork;
        }
        public IActionResult Index()
        {
            IEnumerable<Pacijent> Pacijenti = UnitOfWork.Pacijent.GetAll();
            return View(Pacijenti);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Pacijent p)
        {
            //if (p.JMBG < 1000000000000 || p.JMBG > 10000000000000)
            //{
            //    //Dopuniti nesto nece kada se u klasi Pacijent stavi na JMBG Range 
            //    return View(p);
            //}
            if (ModelState.IsValid)
            {
                var Pacijent = UnitOfWork.Pacijent.GetFirstOrDefault(x => x.BrojKartice == p.BrojKartice || x.Email == p.Email || x.JMBG == p.JMBG);
                if (Pacijent != null)
                {
                    TempData["error"] = "Pacijent sa ovim parametrima vec postoji";
                    return BadRequest("Pacijent sa ovim parametrima vec postoji");
                }
                UnitOfWork.Pacijent.Add(p);
                UnitOfWork.Save();
                TempData["success"] = "Pacijent je uspesno dodat";
                return RedirectToAction("Index");
            }
            return View(p);
        }

        //GET
        public IActionResult Delete(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();
            var p = UnitOfWork.Pacijent.GetFirstOrDefault(x => x.PacijentID == id);
            if (p == null)
                return NotFound();
            return View(p);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Pacijent p)
        {
            UnitOfWork.Pacijent.Remove(p);
            UnitOfWork.Save();
            return RedirectToAction("Index");
        }

        //GET
        public IActionResult Edit(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();
            var p = UnitOfWork.Pacijent.GetFirstOrDefault(x => x.PacijentID == id);
            if (p == null)
                return NotFound();
            return View(p);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Pacijent p)
        {
            UnitOfWork.Pacijent.Update(p);
            UnitOfWork.Save();
            return RedirectToAction("Index");
        }
    }
}

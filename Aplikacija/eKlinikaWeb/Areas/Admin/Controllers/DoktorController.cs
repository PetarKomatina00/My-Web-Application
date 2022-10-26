using eKlinika.DataAccess.Data;
using eKlinika.DataAccess.Repository.IRepository;
using eKlinika.Models;
using eKlinika.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace eKlinikaWeb.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Admin + "," + SD.Direktor)]
    public class DoktorController : Controller
    {
        private readonly IUnitOfWork UnitOfWork;

        public DoktorController(IUnitOfWork _unitofwork)
        {
            UnitOfWork = _unitofwork;
        }
        public IActionResult Index()
        {
            IEnumerable<Doktor> DoctorList = UnitOfWork.Doktor.GetAll();
            return View(DoctorList);


            //IEnumerable<ApplicationUser> listaKorisnika = UnitOfWork.ApplicationUsers.GetAll(x => x.DoktorsIDNumID != null);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Doktor doc)
        {
            var IDNumFromDB = UnitOfWork.DoktorsIDNum.GetFirstOrDefault(x => x.SpecialIDNum == doc.DoktorsIDNumID);
            if(IDNumFromDB == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                UnitOfWork.Doktor.Add(doc);
                UnitOfWork.Save();
                TempData["Success"] = "Doctor Created Successfully";
                return RedirectToAction("Index");
            }
            return View(doc);
        }

        //GET
        public IActionResult Edit(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();
            var Doctor = UnitOfWork.Doktor.GetFirstOrDefault(x => x.DoktorID == id);
            if (Doctor == null)
                return NotFound();
            return View(Doctor);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Doktor doc)
        {
            ModelState.Remove("Password");
            if (ModelState.IsValid)
            {
                UnitOfWork.Doktor.Update(doc);
                UnitOfWork.Save();
                TempData["Success"] = "Doktor je uspesno izmenjen";
                return RedirectToAction("Index");
            }
            return View(doc);
        }
        //GET
        public IActionResult Delete(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();
            var Doktor = UnitOfWork.Doktor.GetFirstOrDefault(x => x.DoktorID == id);
            if (Doktor == null)
                return NotFound();
            return View(Doktor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Doktor doc)
        {
            UnitOfWork.Doktor.Remove(doc);
            UnitOfWork.Save();
            TempData["Success"] = "Doktor deleted successfully";
            return RedirectToAction("Index");
        }
    }
}

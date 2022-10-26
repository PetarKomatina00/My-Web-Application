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
    public class GradController : Controller
    {
        private readonly IUnitOfWork UnitOfWork;

        public GradController(IUnitOfWork _unitofwork)
        {
            UnitOfWork = _unitofwork;
        }
        public IActionResult Index()
        {
            IEnumerable<Grad> DoctorList = UnitOfWork.Grad.GetAll();
            return View(DoctorList);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Grad doc)
        {
            var PostanskiBrojFromDb = UnitOfWork.Grad.GetFirstOrDefault(x => x.PostanskiBroj == doc.PostanskiBroj);
            if(PostanskiBrojFromDb != null)
            {
                return View(doc);
            }
            if (ModelState.IsValid)
            {
                UnitOfWork.Grad.Add(doc);
                UnitOfWork.Save();
                TempData["Success"] = "Pakovanje je uspesno kreirano";
                return RedirectToAction("Index");
            }
            return View(doc);
        }

        //GET
        public IActionResult Edit(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();
            var Doctor = UnitOfWork.Grad.GetFirstOrDefault(x => x.GradID == id);
            if (Doctor == null)
                return NotFound();
            return View(Doctor);
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Grad doc)
        {
            ModelState.Remove("Password");
            if (ModelState.IsValid)
            {
                UnitOfWork.Grad.Update(doc);
                UnitOfWork.Save();
                TempData["Success"] = "Pakovanje je uspesno izmenjeno";
                return RedirectToAction("Index");
            }
            return View(doc);
        }
        //GET
        public IActionResult Delete(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();
            var Doktor = UnitOfWork.Grad.GetFirstOrDefault(x => x.GradID == id);
            if (Doktor == null)
                return NotFound();
            return View(Doktor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Grad doc)
        {
            UnitOfWork.Grad.Remove(doc);
            UnitOfWork.Save();
            TempData["Success"] = "Pakovanje je uspesno obrisano";
            return RedirectToAction("Index");
        }
    }
}

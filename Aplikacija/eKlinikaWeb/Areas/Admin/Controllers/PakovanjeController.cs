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
    public class PakovanjeController : Controller
    {
        private readonly IUnitOfWork UnitOfWork;

        public PakovanjeController(IUnitOfWork _unitofwork)
        {
            UnitOfWork = _unitofwork;
        }
        public IActionResult Index()
        {
            IEnumerable<Pakovanje> DoctorList = UnitOfWork.Pakovanje.GetAll();
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
        public IActionResult Create(Pakovanje doc)
        {
            var PostanskiBrojFromDb = UnitOfWork.Pakovanje.GetFirstOrDefault(x => x.PakovanjeID == doc.PakovanjeID);
            if(PostanskiBrojFromDb != null)
            {
                return View(doc);
            }
            if (ModelState.IsValid)
            {
                UnitOfWork.Pakovanje.Add(doc);
                UnitOfWork.Save();
                TempData["Success"] = "Pakovanje uspesno napravljeno";
                return RedirectToAction("Index");
            }
            return View(doc);
        }

        //GET
        public IActionResult Edit(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();
            var Doctor = UnitOfWork.Pakovanje.GetFirstOrDefault(x => x.PakovanjeID == id);
            if (Doctor == null)
                return NotFound();
            return View(Doctor);
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Pakovanje doc)
        {
            ModelState.Remove("Password");
            if (ModelState.IsValid)
            {
                UnitOfWork.Pakovanje.Update(doc);
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
            var Doktor = UnitOfWork.Pakovanje.GetFirstOrDefault(x => x.PakovanjeID == id);
            if (Doktor == null)
                return NotFound();
            return View(Doktor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Pakovanje doc)
        {
            UnitOfWork.Pakovanje.Remove(doc);
            UnitOfWork.Save();
            TempData["Success"] = "Pakovanje je uspesno obrisano";
            return RedirectToAction("Index");
        }
    }
}

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
    public class VremeController : Controller
    {
        private readonly IUnitOfWork UnitOfWork;

        public VremeController(IUnitOfWork _unitofwork)
        {
            UnitOfWork = _unitofwork;
        }
        public IActionResult Index()
        {
            IEnumerable<Vreme> listaOdeljenja = UnitOfWork.Vreme.GetAll();
            return View(listaOdeljenja);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Vreme doc)
        {
            var PostanskiBrojFromDb = UnitOfWork.Vreme.GetFirstOrDefault(x => x.VremeTimeSpan == doc.VremeTimeSpan);
            if (PostanskiBrojFromDb != null)
            {
                TempData["error"] = "Vreme vec postoji";
                return View(doc);
            }
            if (ModelState.IsValid)
            {
                UnitOfWork.Vreme.Add(doc);
                UnitOfWork.Save();
                TempData["Success"] = "Vreme uspesno napravljeno";
                return RedirectToAction("Index");
            }
            return View(doc);
        }

        //GET
        public IActionResult Edit(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();
            var Doctor = UnitOfWork.Vreme.GetFirstOrDefault(x => x.VremeID == id);
            if (Doctor == null)
                return NotFound();
            return View(Doctor);
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Vreme doc)
        {
            ModelState.Remove("Password");
            if (ModelState.IsValid)
            {
                UnitOfWork.Vreme.Update(doc);
                UnitOfWork.Save();
                TempData["Success"] = "Odeljenje uspesno napravljeno";
                return RedirectToAction("Index");
            }
            return View(doc);
        }
        //GET
        //public IActionResult Delete(int? id)
        //{
        //    if (id == 0 || id == null)
        //        return NotFound();
        //    var Doktor = UnitOfWork.Vreme.GetFirstOrDefault(x => x.VremeID == id);
        //    if (Doktor == null)
        //        return NotFound();
        //    return View(Doktor);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Delete(Vreme doc)
        //{
        //    UnitOfWork.Vreme.Remove(doc);
        //    UnitOfWork.Save();
        //    TempData["Success"] = "Odeljenje uspesno obrisano";
        //    return RedirectToAction("Index");
        //}


        //Get

        public IActionResult ObrisiSve()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("ObrisiSve")]
        public IActionResult ObrisiSvePOST()
        {
            IEnumerable<Vreme> svaVremena = UnitOfWork.Vreme.GetAll();
            UnitOfWork.Vreme.RemoveRange(svaVremena);
            UnitOfWork.Save();
            return RedirectToAction("Index");

        }
    }
}
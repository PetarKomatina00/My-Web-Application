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
    public class OdeljenjeController : Controller
    {
        private readonly IUnitOfWork UnitOfWork;

        public OdeljenjeController(IUnitOfWork _unitofwork)
        {
            UnitOfWork = _unitofwork;
        }
        public IActionResult Index()
        {
            IEnumerable<Odeljenje> listaOdeljenja = UnitOfWork.Odeljenja.GetAll();
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
        public IActionResult Create(Odeljenje doc)
        {
            var PostanskiBrojFromDb = UnitOfWork.Odeljenja.GetFirstOrDefault(x => x.OdeljenjeID == doc.OdeljenjeID);
            if (PostanskiBrojFromDb != null)
            {
                return View(doc);
            }
            if (ModelState.IsValid)
            {
                UnitOfWork.Odeljenja.Add(doc);
                UnitOfWork.Save();
                TempData["Success"] = "Odeljenje uspesno napravljeno";
                return RedirectToAction("Index");
            }
            return View(doc);
        }

        //GET
        public IActionResult Edit(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();
            var Doctor = UnitOfWork.Odeljenja.GetFirstOrDefault(x => x.OdeljenjeID == id);
            if (Doctor == null)
                return NotFound();
            return View(Doctor);
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Odeljenje doc)
        {
            ModelState.Remove("Password");
            if (ModelState.IsValid)
            {
                UnitOfWork.Odeljenja.Update(doc);
                UnitOfWork.Save();
                TempData["Success"] = "Odeljenje uspesno napravljeno";
                return RedirectToAction("Index");
            }
            return View(doc);
        }
        //GET
        public IActionResult Delete(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();
            var Doktor = UnitOfWork.Odeljenja.GetFirstOrDefault(x => x.OdeljenjeID == id);
            if (Doktor == null)
                return NotFound();
            return View(Doktor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Odeljenje doc)
        {
            UnitOfWork.Odeljenja.Remove(doc);
            UnitOfWork.Save();
            TempData["Success"] = "Odeljenje uspesno obrisano";
            return RedirectToAction("Index");
        }
    }
}

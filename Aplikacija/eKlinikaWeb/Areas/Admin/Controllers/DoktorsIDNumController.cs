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
    public class DoktorsIDNumController : Controller
    {
        private readonly IUnitOfWork UnitOfWork;

        public DoktorsIDNumController(IUnitOfWork _unitofwork)
        {
            UnitOfWork = _unitofwork;
        }
        public IActionResult Index()
        {
            IEnumerable<DoktorsIDNum> DoctorList = UnitOfWork.DoktorsIDNum.GetAll();
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
        public IActionResult Create(DoktorsIDNum doc)
        {
            var PostanskiBrojFromDb = UnitOfWork.DoktorsIDNum.GetFirstOrDefault(x => x.ID == doc.ID);
            if(PostanskiBrojFromDb != null)
            {
                return View(doc);
            }
            if (ModelState.IsValid)
            {
                UnitOfWork.DoktorsIDNum.Add(doc);
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
            var Doctor = UnitOfWork.DoktorsIDNum.GetFirstOrDefault(x => x.ID == id);
            if (Doctor == null)
                return NotFound();
            return View(Doctor);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(DoktorsIDNum doc)
        {
            ModelState.Remove("Password");
            if (ModelState.IsValid)
            {
                UnitOfWork.DoktorsIDNum.Update(doc);
                UnitOfWork.Save();
                TempData["Success"] = "Doktor je uspesno izmenjen";
                return RedirectToAction(nameof(Index));
            }
            return View(doc);
        }
        //GET
        public IActionResult Delete(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();
            var Doktor = UnitOfWork.DoktorsIDNum.GetFirstOrDefault(x => x.ID == id);
            if (Doktor == null)
                return NotFound();
            return View(Doktor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(DoktorsIDNum doc)
        {
            UnitOfWork.DoktorsIDNum.Remove(doc);
            UnitOfWork.Save();
            TempData["Success"] = "Doktor deleted successfully";
            return RedirectToAction("Index");
        }
    }
}

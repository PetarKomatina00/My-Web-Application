using eKlinika.DataAccess.Data;
using eKlinika.DataAccess.Repository.IRepository;
using eKlinika.Models;
using eKlinika.Models.ViewModels;
using eKlinika.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace eKlinikaWeb.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Admin + "," + SD.Direktor)]
    public class LekController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;


        public LekController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        //GET
        public IActionResult Upsert(int? id)
        {
            LekVM productVM = new()
            {
                Lek = new(),
                ListaPakovanja = _unitOfWork.Pakovanje.GetAll().Select(i => new SelectListItem
                {
                    Text = i.VrstaPakovanja,
                    Value = i.PakovanjeID.ToString()
                }),
            };

            if (id == null || id == 0)
            {
                //create product
                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;
                return View(productVM);
            }
            else
            {
                productVM.Lek = _unitOfWork.Lekovi.GetFirstOrDefault(u => u.LekID == id);
                return View(productVM);

                //update product
            }
        }
        //Post
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpsertPost(LekVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"slike\lekovi");
                    var extension = Path.GetExtension(file.FileName);

                    if (obj.Lek.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Lek.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Lek.ImageUrl = @"\slike\lekovi\" + fileName + extension;

                }
                if (obj.Lek.LekID == 0)
                {
                    _unitOfWork.Lekovi.Add(obj.Lek);
                }
                else
                {
                    _unitOfWork.Lekovi.Update(obj.Lek);
                }
                _unitOfWork.Save();
                TempData["success"] = "Lek je uspesno napravljen";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Lekovi.GetAll(includeProperties: "Pakovanje");
            return Json(new { data = productList });
        }

        //POST
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Lekovi.GetFirstOrDefault(u => u.LekID == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Greska prilikom brisanja" });
            }

            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Lekovi.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Uspesno obrisano" });

        }
        #endregion
    }
}

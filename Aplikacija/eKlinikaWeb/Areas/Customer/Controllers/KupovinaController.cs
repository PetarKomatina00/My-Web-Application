using eKlinika.DataAccess.Repository.IRepository;
using eKlinika.Models;
using eKlinika.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

namespace eKlinikaWeb.Controllers;
[Area("Customer")]
public class KupovinaController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public KupovinaController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        IEnumerable<Lek> productList = _unitOfWork.Lekovi.GetAll(includeProperties:"Pakovanje");

        return View(productList);
    }

    public IActionResult Detalji(int lekID)
    {
        Kupovina kupovina = new()
        {
            Kolicina = 1,
            LekID = lekID,
            Lek = _unitOfWork.Lekovi.GetFirstOrDefault(u => u.LekID == lekID, includeProperties: "Pakovanje"),
        };

        return View(kupovina);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public IActionResult Detalji(Kupovina kupovina)
    {
        if(kupovina.Kolicina <= 0)
        {
            TempData["error"] = "Uneliste negativnu kolicinu";
            return RedirectToAction(nameof(Index));
        }
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        kupovina.ApplicationUserID = claim.Value;

        Kupovina cartFromDb = _unitOfWork.Kupovina.GetFirstOrDefault(
            u => u.ApplicationUserID == claim.Value && u.LekID == kupovina.LekID);


        if (cartFromDb == null)
        {

            _unitOfWork.Kupovina.Add(kupovina);
            _unitOfWork.Save();
            HttpContext.Session.SetInt32(SD.SessionCart,
                _unitOfWork.Kupovina.GetAll(u => u.ApplicationUserID == claim.Value).ToList().Count);
        }
        else
        {
            _unitOfWork.Kupovina.IncrementCount(cartFromDb, kupovina.Kolicina);
            _unitOfWork.Save();

        }

        
        return RedirectToAction(nameof(Index));
    }


    //public IActionResult Privacy()
    //{
    //    return View();
    //}

    //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    //public IActionResult Error()
    //{
    //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    //}
}

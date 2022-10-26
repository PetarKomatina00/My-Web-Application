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
public class DoktoriController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DoktoriController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        IEnumerable<ApplicationUser> productList = _unitOfWork.ApplicationUsers.GetAll(x => x.ImageUrl != null);
        return View(productList);
    }

    public IActionResult Obrisi(string? id)
    {
        if(id == null)
        {
            return RedirectToAction(nameof(Index));
        }
        ApplicationUser user = _unitOfWork.ApplicationUsers.GetFirstOrDefault(x => x.Id == id);
        if(user != null)
        {
            _unitOfWork.ApplicationUsers.Remove(user);
            _unitOfWork.Save();
            TempData["success"] = "Uspesno obrisan doktor";
        }
        return RedirectToAction(nameof(Index));
    }
}

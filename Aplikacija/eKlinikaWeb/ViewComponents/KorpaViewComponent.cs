using eKlinika.DataAccess.Repository.IRepository;
using eKlinika.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eKlinikaWeb.ViewComponents
{
    public class KorpaViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public KorpaViewComponent(IUnitOfWork unitofWork)
        {
            _unitOfWork = unitofWork;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if(claim != null)
            {
                if(HttpContext.Session.GetInt32(SD.SessionCart) != null)
                {
                    return View(HttpContext.Session.GetInt32(SD.SessionCart));
                }
                else
                {
                    HttpContext.Session.SetInt32(SD.SessionCart,
                        _unitOfWork.Kupovina.GetAll(x => x.ApplicationUserID == claim.Value).ToList().Count);
                    return View(HttpContext.Session.GetInt32(SD.SessionCart));
                }
            }
            else
            {
                HttpContext.Session.Clear();
                return View(0);
            }
        
        }
    }
}

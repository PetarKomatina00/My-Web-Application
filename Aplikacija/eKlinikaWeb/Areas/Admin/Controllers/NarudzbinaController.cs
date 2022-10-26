using eKlinika.DataAccess.Repository.IRepository;
using eKlinika.Models;
using eKlinika.Models.ViewModels;
using eKlinika.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace eKlinikaWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class NarudzbinaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public NarudzbinaVM NarudzbinaVM { get; set; }

        public NarudzbinaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detalji(int narudzbinaID)
        {
            NarudzbinaVM = new NarudzbinaVM()
            {
                Narudzbina = _unitOfWork.Narudzbina.GetFirstOrDefault(x => x.ID == narudzbinaID, includeProperties: "ApplicationUser"),
                DetaljiNarudzbine = _unitOfWork.DetaljiNarudzbine.GetAll(x => x.ID == narudzbinaID, includeProperties: "Lek")
            };
            return View(NarudzbinaVM);
        }
        [ActionName("Detalji")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DetaljiPayNow()
        {
            NarudzbinaVM.Narudzbina = _unitOfWork.Narudzbina.GetFirstOrDefault(x => x.ID == NarudzbinaVM.Narudzbina.ID, includeProperties: "ApplicationUser");
            NarudzbinaVM.DetaljiNarudzbine = _unitOfWork.DetaljiNarudzbine.GetAll(x => x.ID == NarudzbinaVM.Narudzbina.ID, includeProperties: "Lek");

            var domain = "https://localhost:44398/";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                    {
                      "card",
                    },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"Admin/Narudzbina/PaymentConfirmation?narudzbinaid={NarudzbinaVM.Narudzbina.ID}",
                CancelUrl = domain + $"Admin/Narudzbina/Detaljs?narudzbinaID={NarudzbinaVM.Narudzbina.ID}",
            };
            foreach (var item in NarudzbinaVM.DetaljiNarudzbine)
            {

                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Cena * 100),//20.00 -> 2000
                        Currency = "RSD",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Lek.Ime
                        },
                    },
                    Quantity = item.Kolicina,
                };
                options.LineItems.Add(sessionLineItem);

            }

            var service = new SessionService();
            Session session = service.Create(options);
            _unitOfWork.Narudzbina.UpdateStripePaymentID(NarudzbinaVM.Narudzbina.ID, session.Id, session.PaymentIntentId);
            _unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }


        public IActionResult PaymentConfirmation(int narudzbinaid)
        {
            Narudzbina orderHeader = _unitOfWork.Narudzbina.GetFirstOrDefault(u => u.ID == narudzbinaid, includeProperties: "ApplicationUser");
            if (orderHeader.statusPlacanja == SD.PaymentStatusDelayedPayment)
            {
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionID);
                //check the stripe status
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.Narudzbina.UpdateStatus(narudzbinaid, orderHeader.statusIsporuke, SD.PaymentStatusApproved);
                    _unitOfWork.Save();
                }
            }
            return View(narudzbinaid);
        }

        [HttpPost]
        [Authorize(Roles = SD.Admin + "," + SD.Doktor)]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateOrderDetail(int narudzbinaID)
        {
            var NarudzbinaFromDB = _unitOfWork.Narudzbina.GetFirstOrDefault(x => x.ID == NarudzbinaVM.Narudzbina.ID);
            NarudzbinaFromDB.Ime = NarudzbinaVM.Narudzbina.Ime;
            NarudzbinaFromDB.BrojTelefona = NarudzbinaVM.Narudzbina.BrojTelefona;
            NarudzbinaFromDB.Adresa = NarudzbinaVM.Narudzbina.Adresa;
            NarudzbinaFromDB.Grad = NarudzbinaVM.Narudzbina.Grad;
            NarudzbinaFromDB.PostanskiBroj = NarudzbinaVM.Narudzbina.PostanskiBroj;

            if (NarudzbinaVM.Narudzbina.Dostavljac != null)
            {
                NarudzbinaFromDB.Dostavljac = NarudzbinaVM.Narudzbina.Dostavljac;
            }
            _unitOfWork.Narudzbina.Update(NarudzbinaFromDB);
            _unitOfWork.Save();
            TempData["success"] = "Uspesno izmenjena Narudzbina";
            return RedirectToAction("Detalji", "Narudzbina", new { narudzbinaID = NarudzbinaFromDB.ID });
        }

        [HttpPost]
        [Authorize(Roles = SD.Admin + "," + SD.Doktor)]

        [ValidateAntiForgeryToken]
        public IActionResult StartProcessing(int narudzbinaID)
        {
            _unitOfWork.Narudzbina.UpdateStatus(NarudzbinaVM.Narudzbina.ID, SD.StatusInProcess);
            _unitOfWork.Save();
            TempData["success"] = "Uspesno startovan proces";
            return RedirectToAction("Detalji", "Narudzbina", new { narudzbinaID = NarudzbinaVM.Narudzbina.ID });
        }

        [HttpPost]
        [Authorize(Roles = SD.Admin + "," + SD.Doktor)]
        [ValidateAntiForgeryToken]
        public IActionResult ShipOrder(int narudzbinaID)
        {
            var NarudzbinaFromDB = _unitOfWork.Narudzbina.GetFirstOrDefault(x => x.ID == NarudzbinaVM.Narudzbina.ID);
            NarudzbinaFromDB.Dostavljac = NarudzbinaVM.Narudzbina.Dostavljac;
            NarudzbinaFromDB.statusIsporuke = SD.StatusShipped;
            NarudzbinaFromDB.DatumIsporuke = DateTime.Now;
            _unitOfWork.Narudzbina.Update(NarudzbinaFromDB);
            _unitOfWork.Save();
            TempData["success"] = "Uspesno dostavljeno!";
            return RedirectToAction("Detalji", "Narudzbina", new { narudzbinaID = NarudzbinaVM.Narudzbina.ID });
        }
        [HttpPost]
        [Authorize(Roles = SD.Admin + "," + SD.Doktor)]
        [ValidateAntiForgeryToken]
        public IActionResult CancelOrder(int narudzbinaID)
        {
            var NarudzbinaFromDB = _unitOfWork.Narudzbina.GetFirstOrDefault(x => x.ID == NarudzbinaVM.Narudzbina.ID);
            if(NarudzbinaFromDB.statusPlacanja == SD.PaymentStatusApproved)
            {
                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = NarudzbinaFromDB.PaymentIntentID
                };
                var service = new RefundService();
                Refund refund = service.Create(options);

                _unitOfWork.Narudzbina.UpdateStatus(NarudzbinaFromDB.ID, SD.StatusCancelled, SD.StatusRefunded);
            }
            else
            {
                _unitOfWork.Narudzbina.UpdateStatus(NarudzbinaFromDB.ID, SD.StatusCancelled, SD.StatusCancelled);
            }
            _unitOfWork.Save();
            TempData["success"] = "Uspesno otkazano!";
            return RedirectToAction("Detalji", "Narudzbina", new { narudzbinaID = NarudzbinaVM.Narudzbina.ID });
        }
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<Narudzbina> listaNarudzbina;

            if (User.IsInRole(SD.Admin))
            {
                listaNarudzbina = _unitOfWork.Narudzbina.GetAll(includeProperties: "ApplicationUser");
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                listaNarudzbina = _unitOfWork.Narudzbina.GetAll(x => x.ApplicationUserID == claim.Value, includeProperties: "ApplicationUser");
            }
            switch (status)
            {
                case "inprocess":
                    listaNarudzbina = listaNarudzbina.Where(x => x.statusPlacanja == SD.StatusInProcess);
                    break;
                case "completed":
                    listaNarudzbina = listaNarudzbina.Where(x => x.statusPlacanja == SD.StatusShipped);
                    break;
                case "approved":
                    listaNarudzbina = listaNarudzbina.Where(x => x.statusPlacanja == SD.StatusApproved);
                    break;
                case "cancelled":
                    listaNarudzbina = listaNarudzbina.Where(x => x.statusIsporuke == SD.StatusCancelled);
                    break;
                default:
                    break;
            }
            return Json(new {data = listaNarudzbina });
        }
        #endregion
    }
}

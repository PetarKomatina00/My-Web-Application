using eKlinika.DataAccess.Repository.IRepository;
using eKlinika.Models;
using eKlinika.Models.ViewModels;
using eKlinika.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;

namespace eKlinikaWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class KorpaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        [BindProperty]
        public KupovinaVM kupovinaVM { get; set; }

        public int OrderTotal { get; set; }
        public KorpaController(IUnitOfWork unitOfWork , IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            kupovinaVM  = new KupovinaVM()
            {
                listaKupovine = _unitOfWork.Kupovina.GetAll(u => u.ApplicationUserID == claim.Value,
                includeProperties: "Lek"),
                Narudzbina = new()
            };
            foreach (var cart in kupovinaVM.listaKupovine)
            {
                cart.Cena = GetPriceBasedOnQuantity(cart.Kolicina, cart.Lek.Cena,cart.Lek.Cena3);
                kupovinaVM.Narudzbina.ukupnaSuma += (cart.Cena * cart.Kolicina);
            }
            return View(kupovinaVM);
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            kupovinaVM = new KupovinaVM()
            {
                listaKupovine = _unitOfWork.Kupovina.GetAll(u => u.ApplicationUserID == claim.Value,
                includeProperties: "Lek"),
                Narudzbina = new()
            };
            kupovinaVM.Narudzbina.ApplicationUser = _unitOfWork.ApplicationUsers.GetFirstOrDefault(
                u => u.Id == claim.Value);

            kupovinaVM.Narudzbina.Ime = kupovinaVM.Narudzbina.ApplicationUser.Ime;
            kupovinaVM.Narudzbina.BrojTelefona = kupovinaVM.Narudzbina.ApplicationUser.BrojTelefona;
            kupovinaVM.Narudzbina.Adresa = kupovinaVM.Narudzbina.ApplicationUser.Adresa;
            kupovinaVM.Narudzbina.Grad = (kupovinaVM.Narudzbina.ApplicationUser.NazivGrada);
            //string nazivGrada = _unitOfWork.Grad.GetFirstOrDefault(x => x.GradID == gradID).NazivGrada;
            kupovinaVM.Narudzbina.PostanskiBroj = kupovinaVM.Narudzbina.ApplicationUser.PostanskiBroj;


            foreach (var cart in kupovinaVM.listaKupovine)
            {
                cart.Cena = GetPriceBasedOnQuantity(cart.Kolicina, cart.Lek.Cena,
                    cart.Lek.Cena3);
                kupovinaVM.Narudzbina.ukupnaSuma += (cart.Cena * cart.Kolicina);
            }
            return View(kupovinaVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            kupovinaVM.listaKupovine = _unitOfWork.Kupovina.GetAll(u => u.ApplicationUserID == claim.Value,
                includeProperties: "Lek");


            kupovinaVM.Narudzbina.DatumNarucivanja = System.DateTime.Now;
            kupovinaVM.Narudzbina.ApplicationUserID = claim.Value;

            foreach (var cart in kupovinaVM.listaKupovine)
            {
                cart.Cena = GetPriceBasedOnQuantity(cart.Kolicina, cart.Lek.Cena,
                    cart.Lek.Cena3);
                kupovinaVM.Narudzbina.ukupnaSuma += (cart.Cena * cart.Kolicina);
            }
            //    ApplicationUser applicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);

            //    if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            //    {
            //        ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            //        ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            //    }
            //    else
            //    {
            //        ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
            //        ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
            //    }

            _unitOfWork.Narudzbina.Add(kupovinaVM.Narudzbina);
            _unitOfWork.Save();
            foreach (var cart in kupovinaVM.listaKupovine)
            {
                DetaljiNarudzbine orderDetail = new()
                {
                    LekID = cart.LekID,
                    NarudzbinaID = kupovinaVM.Narudzbina.ID,
                    Cena = cart.Cena,
                    Kolicina = cart.Kolicina
                };
                _unitOfWork.DetaljiNarudzbine.Add(orderDetail);
                _unitOfWork.Save();
            }
            //    if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            //    {
            //stripe settings 
            var domain = "https://localhost:44398/";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                    {
                      "card",
                    },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"Customer/Korpa/OrderConfirmation?id={kupovinaVM.Narudzbina.ID}",
                CancelUrl = domain + $"Customer/Korpa/Index",
            };
            foreach (var item in kupovinaVM.listaKupovine)
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
            _unitOfWork.Narudzbina.UpdateStripePaymentID(kupovinaVM.Narudzbina.ID, session.Id, session.PaymentIntentId);
            _unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
            //    }

            //    else
            //    {
            //        return RedirectToAction("OrderConfirmation", "Cart", new { id = ShoppingCartVM.OrderHeader.Id });
            //    }
        }

        public IActionResult OrderConfirmation(int id)
        {
            Narudzbina orderHeader = _unitOfWork.Narudzbina.GetFirstOrDefault(u => u.ID == id, includeProperties: "ApplicationUser");
            if (orderHeader.statusPlacanja != SD.PaymentStatusDelayedPayment)
            {
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionID);
                //check the stripe status
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.Narudzbina.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                    _unitOfWork.Save();
                }
            }
            string email = orderHeader.ApplicationUser.NormalizedEmail;
            //_emailSender.SendEmailAsync(email.ToLower(), "Porudzbina- eKlinika", "<p>Narudzbina je uspesno napravljena</p>"); ;
            List<Kupovina> shoppingCarts = _unitOfWork.Kupovina.GetAll(u => u.ApplicationUserID ==
            orderHeader.ApplicationUserID).ToList();
            HttpContext.Session.Clear();
            _unitOfWork.Kupovina.RemoveRange(shoppingCarts);
            _unitOfWork.Save();
            return View(id);
        }

        public IActionResult plus(int korpaID)
        {
            var cart = _unitOfWork.Kupovina.GetFirstOrDefault(u => u.KupovinaID == korpaID);
            _unitOfWork.Kupovina.IncrementCount(cart, 1);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult minus(int korpaID)
        {
            var cart = _unitOfWork.Kupovina.GetFirstOrDefault(u => u.KupovinaID == korpaID);
            if (cart.Kolicina <= 1)
            {
                _unitOfWork.Kupovina.Remove(cart);
                var count = _unitOfWork.Kupovina.GetAll(u => u.ApplicationUserID == cart.ApplicationUserID).ToList().Count - 1;
                HttpContext.Session.SetInt32(SD.SessionCart, count);
            }
            else
            {
                _unitOfWork.Kupovina.DecrementCount(cart, 1);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult remove(int korpaID)
        {
            var cart = _unitOfWork.Kupovina.GetFirstOrDefault(u => u.KupovinaID == korpaID);
            _unitOfWork.Kupovina.Remove(cart);
            _unitOfWork.Save();
            var count = _unitOfWork.Kupovina.GetAll(u => u.ApplicationUserID == cart.ApplicationUserID).ToList().Count;
            HttpContext.Session.SetInt32(SD.SessionCart, count);
            return RedirectToAction(nameof(Index));
        }
        private double GetPriceBasedOnQuantity(double kolicina, double cena, double cena3)
        {
            if (kolicina <= 2)
            {
                return cena;
            }
            else
            {
                return cena3;
            }
        }
    }
}

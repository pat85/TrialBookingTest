using Microsoft.AspNetCore.Mvc;
using TrialBookingApp.Web.Business.Interfaces.Services;

namespace TrialBookingApp.Web.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var payments = await _paymentService.GetAllPaymentDetailsAsync();

            return View(payments);
        }
    }
}

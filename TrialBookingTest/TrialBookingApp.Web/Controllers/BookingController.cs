using Microsoft.AspNetCore.Mvc;
using TrialBookingApp.Web.Business.Interfaces.Services;
using TrialBookingApp.Web.Domain.Enums;

namespace TrialBookingApp.Web.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var bookings = await _bookingService.GetAllBookingDetailsAsync();

            return View(bookings);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var booking = await _bookingService.GetBookingDetailsAsync(id);

            return booking is null
                ? NotFound()
                : View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompletePayment(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var result = await _bookingService.CompletePaymentAsync(id);

            if (result == PaymentCompletionResult.NotFound)
            {
                return NotFound();
            }

            TempData[result switch
            {
                PaymentCompletionResult.Success => "BookingMessage",
                PaymentCompletionResult.ClassFull => "BookingError",
                _ => "BookingError"
            }] = result switch
            {
                PaymentCompletionResult.Success => "Congratulations! Your booking has been confirmed.",
                PaymentCompletionResult.ClassFull => "The class is full. Your payment could not be completed.",
                _ => "Payment for this booking has already been processed."
            };

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelBooking(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var cancelled = await _bookingService.CancelBookingAsync(id);

            if (!cancelled)
            {
                return NotFound();
            }

            TempData["BookingMessage"] = "The booking has been canceled.";

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}

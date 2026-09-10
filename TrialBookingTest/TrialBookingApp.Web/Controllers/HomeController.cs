using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TrialBookingApp.Web.Business.Interfaces.Services;
using TrialBookingApp.Web.Domain.Enums;
using TrialBookingApp.Web.Models;

namespace TrialBookingApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IParentService _parentService;
        private readonly IStudentService _studentService;
        private readonly ITrialClassService _trialClassService;
        private readonly IBookingService _bookingService;

        public HomeController(
            IParentService parentService,
            IStudentService studentService,
            ITrialClassService trialClassService,
            IBookingService bookingService)
        {
            _parentService = parentService;
            _studentService = studentService;
            _trialClassService = trialClassService;
            _bookingService = bookingService;
        }

        public async Task<IActionResult> Index()
        {
            var parents = await _parentService.GetAllParentsAsync();
            var trialClasses = await _trialClassService.GetAllTrialClassesAsync();

            return View(new HomeViewModel
            {
                Parents = parents,
                TrialClasses = trialClasses
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitBooking(Guid studentId, Guid trialClassId)
        {
            if (studentId == Guid.Empty || trialClassId == Guid.Empty)
            {
                TempData["BookingError"] = "Please select a child and a trial class.";
                return RedirectToAction(nameof(Index));
            }

            var submission = await _bookingService
                .SubmitBookingAsync(studentId, trialClassId);

            if (submission.Result == BookingSubmissionResult.Duplicate)
            {
                TempData["BookingWarning"] = "The student already has a booking for the selected class.";
                return RedirectToAction(nameof(Index));
            }

            if (submission.BookingId is null)
            {
                TempData["BookingError"] = "The booking could not be submitted. It may already exist or the selection is invalid.";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("Details", "Booking", new { id = submission.BookingId.Value });
        }

        [HttpGet]
        public async Task<IActionResult> GetChildren(Guid parentId)
        {
            if (parentId == Guid.Empty)
            {
                return BadRequest();
            }

            var children = await _studentService.GetStudentsByParentIdAsync(parentId);

            return Json(children.Select(child => new
            {
                id = child.StudentId,
                name = child.StudentName
            }));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using TrialBookingApp.Web.Business.Interfaces.Repositories;
using TrialBookingApp.Web.Business.Interfaces.Services;
using TrialBookingApp.Web.DataAccess;
using TrialBookingApp.Web.Domain.Entities;
using TrialBookingApp.Web.Domain.Enums;
using TrialBookingApp.Web.Models;

namespace TrialBookingApp.Web.Business.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IParentRepository _parentRepository;
        private readonly IPaymentAttemptRepository _paymentAttemptRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ITrialClassRepository _trialClassRepository;
        private readonly IPaymentService _paymentService;
        private readonly AppDbContext _context;

        public BookingService(
            IBookingRepository bookingRepository,
            IParentRepository parentRepository,
            IPaymentAttemptRepository paymentAttemptRepository,
            IStudentRepository studentRepository,
            ITrialClassRepository trialClassRepository,
            IPaymentService paymentService,
            AppDbContext context)
        {
            _bookingRepository = bookingRepository;
            _parentRepository = parentRepository;
            _paymentAttemptRepository = paymentAttemptRepository;
            _studentRepository = studentRepository;
            _trialClassRepository = trialClassRepository;
            _paymentService = paymentService;
            _context = context;
        }

        public async Task<(BookingSubmissionResult Result, Guid? BookingId)> SubmitBookingAsync(
            Guid studentId,
            Guid trialClassId)
        {
            var student = await _studentRepository.GetByIdAsync(studentId);
            var trialClass = await _trialClassRepository.GetByIdAsync(trialClassId);

            if (student is null || trialClass is null)
            {
                return (BookingSubmissionResult.InvalidSelection, null);
            }

            var existingBooking = await _bookingRepository
                .GetByStudentAndClassAsync(studentId, trialClassId);

            if (existingBooking is not null)
            {
                return (BookingSubmissionResult.Duplicate, null);
            }

            var booking = new Booking
            {
                BookingId = Guid.NewGuid(),
                StudentId = studentId,
                TrialClassId = trialClassId,
                Status = BookingStatus.PendingPayment,
                CreatedOn = DateTime.UtcNow
            };

            await _bookingRepository.AddAsync(booking);


            await _context.SaveChangesAsync();

            return (BookingSubmissionResult.Success, booking.BookingId);
        }

        public async Task<List<BookingDetailsViewModel>> GetAllBookingDetailsAsync()
        {
            var bookings = await _bookingRepository.GetAllAsync();
            var bookingDetails = new List<BookingDetailsViewModel>();

            foreach (var booking in bookings)
            {
                var student = await _studentRepository.GetByIdAsync(booking.StudentId);
                var trialClass = await _trialClassRepository.GetByIdAsync(booking.TrialClassId);
                var parent = student is null
                    ? null
                    : await _parentRepository.GetByIdAsync(student.ParentId);

                bookingDetails.Add(new BookingDetailsViewModel
                {
                    BookingId = booking.BookingId,
                    ParentName = parent?.ParentName ?? "Unknown",
                    StudentName = student?.StudentName ?? "Unknown",
                    TrialClassName = trialClass?.TrialClassTitle ?? "Unknown",
                    Status = booking.Status
                });
            }

            return bookingDetails;
        }

        public async Task<PaymentCompletionResult> CompletePaymentAsync(Guid bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);

            if (booking is null)
            {
                return PaymentCompletionResult.NotFound;
            }

            if (booking.Status != BookingStatus.PendingPayment)
            {
                return PaymentCompletionResult.AlreadyProcessed;
            }

            var trialClass = await _trialClassRepository
                .GetByIdAsync(booking.TrialClassId);

            if (trialClass is null)
            {
                return PaymentCompletionResult.NotFound;
            }

            const decimal amount = 0m;
            var canComplete = trialClass.ConfirmedBookingCount < trialClass.TrialClassCapacity;
            var paymentStatus = await _paymentService
                .ProcessPaymentAsync(amount, canComplete);

            var paymentAttempt = new PaymentAttempt
            {
                PaymentAttemptId = Guid.NewGuid(),
                BookingId = booking.BookingId,
                Status = paymentStatus,
                Amount = amount,
                AttemptDate = DateTime.UtcNow,
                CreatedOn = DateTime.UtcNow
            };

            await _paymentAttemptRepository.AddAsync(paymentAttempt);

            if (paymentStatus == PaymentStatus.Success)
            {
                booking.Status = BookingStatus.Confirmed;
                booking.ConfirmationDate = DateTime.UtcNow;
                _bookingRepository.Update(booking);

                trialClass.ConfirmedBookingCount++;
                _trialClassRepository.Update(trialClass);

                await _context.SaveChangesAsync();

                return PaymentCompletionResult.Success;
            }

            booking.Status = BookingStatus.PaymentFailed;
            _bookingRepository.Update(booking);

            await _context.SaveChangesAsync();

            return PaymentCompletionResult.ClassFull;
        }

        public async Task<bool> CancelBookingAsync(Guid bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);

            if (booking is null || booking.Status == BookingStatus.Cancelled)
            {
                return false;
            }

            booking.Status = BookingStatus.Cancelled;
            _bookingRepository.Update(booking);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<BookingDetailsViewModel?> GetBookingDetailsAsync(Guid bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);

            if (booking is null)
            {
                return null;
            }

            var student = await _studentRepository.GetByIdAsync(booking.StudentId);
            var trialClass = await _trialClassRepository.GetByIdAsync(booking.TrialClassId);
            var parent = student is null
                ? null
                : await _parentRepository.GetByIdAsync(student.ParentId);

            if (student is null || parent is null || trialClass is null)
            {
                return null;
            }

            return new BookingDetailsViewModel
            {
                BookingId = booking.BookingId,
                ParentName = parent.ParentName,
                StudentName = student.StudentName,
                TrialClassName = trialClass.TrialClassTitle,
                Status = booking.Status
            };
        }
    }
}

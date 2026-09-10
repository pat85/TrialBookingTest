using TrialBookingApp.Web.Business.Interfaces.Repositories;
using TrialBookingApp.Web.Business.Interfaces.Services;
using TrialBookingApp.Web.Domain.Enums;
using TrialBookingApp.Web.Models;

namespace TrialBookingApp.Web.Business.Services
{
    public class MockPaymentService : IPaymentService
    {
        private readonly IPaymentAttemptRepository _paymentAttemptRepository;

        public MockPaymentService(IPaymentAttemptRepository paymentAttemptRepository)
        {
            _paymentAttemptRepository = paymentAttemptRepository;
        }

        public async Task<List<PaymentDetailsViewModel>> GetAllPaymentDetailsAsync()
        {
            var paymentAttempts = await _paymentAttemptRepository.GetAllAsync();

            return paymentAttempts.Select(payment => new PaymentDetailsViewModel
            {
                PaymentId = payment.PaymentAttemptId,
                BookingId = payment.BookingId,
                ParentName = payment.Booking?.Student?.Parent?.ParentName ?? "Unknown",
                StudentName = payment.Booking?.Student?.StudentName ?? "Unknown",
                TrialClassName = payment.Booking?.TrialClass?.TrialClassTitle ?? "Unknown",
                Status = payment.Status
            }).ToList();
        }

        public Task<PaymentStatus> ProcessPaymentAsync(decimal amount, bool canComplete)
        {
            _ = amount;

            return Task.FromResult(
                canComplete ? PaymentStatus.Success : PaymentStatus.Failed);
        }
    }
}

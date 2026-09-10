using System;
using System.Collections.Generic;
using System.Text;
using TrialBookingApp.Web.Domain.Enums;
using TrialBookingApp.Web.Models;

namespace TrialBookingApp.Web.Business.Interfaces.Services
{
    public interface IBookingService
    {
        Task<(BookingSubmissionResult Result, Guid? BookingId)> SubmitBookingAsync(
            Guid studentId,
            Guid trialClassId);
        Task<List<BookingDetailsViewModel>> GetAllBookingDetailsAsync();
        Task<BookingDetailsViewModel?> GetBookingDetailsAsync(Guid bookingId);
        Task<PaymentCompletionResult> CompletePaymentAsync(Guid bookingId);
        Task<bool> CancelBookingAsync(Guid bookingId);
    }
}

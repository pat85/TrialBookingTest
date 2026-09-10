using System;
using TrialBookingApp.Web.Domain.Enums;

namespace TrialBookingApp.Web.Models
{
    public class BookingDetailsViewModel
    {
        public Guid BookingId { get; set; }
        public string ParentName { get; set; } = null!;
        public string StudentName { get; set; } = null!;
        public string TrialClassName { get; set; } = null!;
        public BookingStatus Status { get; set; }

        public bool CanCompletePayment => Status == BookingStatus.PendingPayment;

        public bool CanCancelBooking => Status != BookingStatus.Cancelled;
    }
}

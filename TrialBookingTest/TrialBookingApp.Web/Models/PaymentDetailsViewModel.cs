using System;
using TrialBookingApp.Web.Domain.Enums;

namespace TrialBookingApp.Web.Models
{
    public class PaymentDetailsViewModel
    {
        public Guid PaymentId { get; set; }

        public Guid BookingId { get; set; }

        public string ParentName { get; set; } = null!;

        public string StudentName { get; set; } = null!;

        public string TrialClassName { get; set; } = null!;

        public PaymentStatus Status { get; set; }
    }
}

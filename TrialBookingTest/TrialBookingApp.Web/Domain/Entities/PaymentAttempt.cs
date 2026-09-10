using System;
using System.Collections.Generic;
using System.Text;
using TrialBookingApp.Web.Domain.Enums;

namespace TrialBookingApp.Web.Domain.Entities
{
    public class PaymentAttempt : BaseEntity
    {
        public Guid PaymentAttemptId { get; set; }

        public Guid BookingId { get; set; }

        public PaymentStatus Status { get; set; }

        public decimal Amount { get; set; }

        public DateTime AttemptDate { get; set; }

        public string? TransactionRef { get; set; }

        public Booking Booking { get; set; } = null!;
    }
}

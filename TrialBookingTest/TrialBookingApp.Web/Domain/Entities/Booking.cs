using System;
using System.Collections.Generic;
using System.Text;
using TrialBookingApp.Web.Domain.Enums;

namespace TrialBookingApp.Web.Domain.Entities
{
    public class Booking : BaseEntity
    {
        public Guid BookingId { get; set; }

        public Guid StudentId { get; set; }
        public Guid TrialClassId { get; set; }

        public BookingStatus Status { get; set; }

        public DateTime? ConfirmationDate { get; set; }

        public Student Student { get; set; } = null!;
        public TrialClass TrialClass { get; set; } = null!;

        public ICollection<PaymentAttempt> PaymentAttempts { get; set; }
            = new List<PaymentAttempt>();
    }
}

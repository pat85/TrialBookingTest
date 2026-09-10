using System;
using System.Collections.Generic;
using System.Text;

namespace TrialBookingApp.Web.Domain.Entities
{
    public class TrialClass : BaseEntity
    {
        public Guid TrialClassId { get; set; }

        public string TrialClassTitle { get; set; } = null!;

        public DateTime TrialClassStartDate { get; set; }
        public DateTime TrialClassEndDate { get; set; }

        public int TrialClassCapacity { get; set; } = 4;

        public int ConfirmedBookingCount { get; set; } = 0;

        public ICollection<Booking> TrialClassBookings { get; set; } = new List<Booking>();
    }
}

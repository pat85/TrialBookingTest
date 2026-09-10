using System;
using System.Collections.Generic;
using System.Text;

namespace TrialBookingApp.Web.Domain.Entities
{
    public class Student : BaseEntity
    {
        public Guid StudentId { get; set; }

        public Guid ParentId { get; set; }
        public string StudentName { get; set; } = null!;

        public Parent Parent { get; set; } = null!;

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}

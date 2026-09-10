using System;
using System.Collections.Generic;
using System.Text;

namespace TrialBookingApp.Web.Domain.Entities
{
    public class Parent : BaseEntity
    {
        public Guid ParentId { get; set; }

        public string ParentName { get; set; } = null!;
        public string ParentEmail { get; set; } = null!;

        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}

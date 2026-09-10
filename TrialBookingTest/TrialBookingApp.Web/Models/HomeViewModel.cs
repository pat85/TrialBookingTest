using TrialBookingApp.Web.Domain.Entities;

namespace TrialBookingApp.Web.Models
{
    public class HomeViewModel
    {
        public List<Parent> Parents { get; set; } = new();
        public List<TrialClass> TrialClasses { get; set; } = new();
    }
}

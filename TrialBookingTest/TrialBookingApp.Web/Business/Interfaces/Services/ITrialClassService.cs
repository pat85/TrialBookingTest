using TrialBookingApp.Web.Domain.Entities;

namespace TrialBookingApp.Web.Business.Interfaces.Services
{
    public interface ITrialClassService
    {
        Task<List<TrialClass>> GetAllTrialClassesAsync();
    }
}

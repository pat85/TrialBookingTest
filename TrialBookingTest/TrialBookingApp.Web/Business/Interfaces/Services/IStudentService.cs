using TrialBookingApp.Web.Domain.Entities;

namespace TrialBookingApp.Web.Business.Interfaces.Services
{
    public interface IStudentService
    {
        Task<List<Student>> GetStudentsByParentIdAsync(Guid parentId);
    }
}

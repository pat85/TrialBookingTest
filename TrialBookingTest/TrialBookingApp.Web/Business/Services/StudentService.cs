using TrialBookingApp.Web.Business.Interfaces.Repositories;
using TrialBookingApp.Web.Business.Interfaces.Services;
using TrialBookingApp.Web.Domain.Entities;

namespace TrialBookingApp.Web.Business.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<List<Student>> GetStudentsByParentIdAsync(Guid parentId)
        {
            return await _studentRepository.GetByParentIdAsync(parentId);
        }
    }
}

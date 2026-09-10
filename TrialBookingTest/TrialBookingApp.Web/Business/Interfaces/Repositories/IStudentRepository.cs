using System;
using System.Collections.Generic;
using System.Text;
using TrialBookingApp.Web.Domain.Entities;

namespace TrialBookingApp.Web.Business.Interfaces.Repositories
{
    public interface IStudentRepository
    {
        Task<Student?> GetByIdAsync(Guid id);
        Task<List<Student>> GetAllAsync();
        Task<List<Student>> GetByParentIdAsync(Guid parentId);

        Task AddAsync(Student student);
        void Update(Student student);
        void Delete(Student student);

        Task<bool> ExistsAsync(Guid id);
    }
}

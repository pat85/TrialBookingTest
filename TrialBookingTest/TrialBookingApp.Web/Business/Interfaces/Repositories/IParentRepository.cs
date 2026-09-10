using System;
using System.Collections.Generic;
using System.Text;
using TrialBookingApp.Web.Domain.Entities;

namespace TrialBookingApp.Web.Business.Interfaces.Repositories
{
    public interface IParentRepository
    {
        Task<Parent?> GetByIdAsync(Guid id);

        Task<List<Parent>> GetAllAsync();

        Task AddAsync(Parent parent);

        void Update(Parent parent);

        void Delete(Parent parent);

        Task<bool> ExistsAsync(Guid id);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using TrialBookingApp.Web.Domain.Entities;

namespace TrialBookingApp.Web.Business.Interfaces.Repositories
{
    public interface ITrialClassRepository
    {
        Task<TrialClass?> GetByIdAsync(Guid id);

        Task<List<TrialClass>> GetAllAsync();

        Task<List<TrialClass>> GetAvailableAsync();

        Task AddAsync(TrialClass trialClass);

        void Update(TrialClass trialClass);

        void Delete(TrialClass trialClass);
    }
}

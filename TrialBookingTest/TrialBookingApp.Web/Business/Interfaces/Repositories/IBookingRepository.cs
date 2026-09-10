using System;
using System.Collections.Generic;
using System.Text;
using TrialBookingApp.Web.Domain.Entities;

namespace TrialBookingApp.Web.Business.Interfaces.Repositories
{
    public interface IBookingRepository
    {
        Task<Booking?> GetByIdAsync(Guid id);

        Task<List<Booking>> GetAllAsync();

        Task<Booking?> GetByStudentAndClassAsync(
            Guid studentId,
            Guid trialClassId);

        Task<List<Booking>> GetByTrialClassAsync(
            Guid trialClassId);

        Task<int> GetConfirmedCountAsync(
            Guid trialClassId);

        Task AddAsync(Booking booking);

        void Update(Booking booking);

        void Delete(Booking booking);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TrialBookingApp.Web.Business.Interfaces.Repositories;
using TrialBookingApp.Web.Domain.Entities;
using TrialBookingApp.Web.Domain.Enums;

namespace TrialBookingApp.Web.DataAccess.Repositories
{
    public class TrialClassRepository : ITrialClassRepository
    {
        private readonly AppDbContext _context;

        public TrialClassRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TrialClass?> GetByIdAsync(Guid id)
        {
            return await _context.TrialClasses
                .FirstOrDefaultAsync(x => x.TrialClassId == id);
        }

        public async Task<List<TrialClass>> GetAllAsync()
        {
            return await _context.TrialClasses
                .ToListAsync();
        }

        public async Task<List<TrialClass>> GetAvailableAsync()
        {
            var now = DateTime.UtcNow;

            return await _context.TrialClasses
                .Where(x =>
                    x.TrialClassStartDate > now &&
                    x.TrialClassBookings.Count(booking =>
                        booking.Status == BookingStatus.Confirmed) < x.TrialClassCapacity)
                .ToListAsync();
        }

        public async Task AddAsync(TrialClass trialClass)
        {
            await _context.TrialClasses.AddAsync(trialClass);
        }

        public void Update(TrialClass trialClass)
        {
            _context.TrialClasses.Update(trialClass);
        }

        public void Delete(TrialClass trialClass)
        {
            _context.TrialClasses.Remove(trialClass);
        }
    }
}

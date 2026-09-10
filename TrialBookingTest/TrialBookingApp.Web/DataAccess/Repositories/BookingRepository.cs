using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TrialBookingApp.Web.Business.Interfaces.Repositories;
using TrialBookingApp.Web.Domain.Entities;
using TrialBookingApp.Web.Domain.Enums;

namespace TrialBookingApp.Web.DataAccess.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _context;

        public BookingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Booking?> GetByIdAsync(Guid id)
        {
            return await _context.Bookings
                .FirstOrDefaultAsync(x => x.BookingId == id);
        }

        public async Task<List<Booking>> GetAllAsync()
        {
            return await _context.Bookings
                .ToListAsync();
        }

        public async Task<Booking?> GetByStudentAndClassAsync(
            Guid studentId,
            Guid trialClassId)
        {
            return await _context.Bookings
                .FirstOrDefaultAsync(x =>
                    x.StudentId == studentId &&
                    x.TrialClassId == trialClassId);
        }

        public async Task<List<Booking>> GetByTrialClassAsync(Guid trialClassId)
        {
            return await _context.Bookings
                .Where(x => x.TrialClassId == trialClassId)
                .ToListAsync();
        }

        public async Task<int> GetConfirmedCountAsync(Guid trialClassId)
        {
            return await _context.Bookings
                .CountAsync(x =>
                    x.TrialClassId == trialClassId &&
                    x.Status == BookingStatus.Confirmed);
        }

        public async Task AddAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
        }

        public void Update(Booking booking)
        {
            _context.Bookings.Update(booking);
        }

        public void Delete(Booking booking)
        {
            _context.Bookings.Remove(booking);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TrialBookingApp.Web.Business.Interfaces.Repositories;
using TrialBookingApp.Web.Domain.Entities;

namespace TrialBookingApp.Web.DataAccess.Repositories
{
    public class PaymentAttemptRepository : IPaymentAttemptRepository
    {
        private readonly AppDbContext _context;

        public PaymentAttemptRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PaymentAttempt>> GetAllAsync()
        {
            return await _context.PaymentAttempts
                .Include(x => x.Booking)
                    .ThenInclude(x => x.Student)
                        .ThenInclude(x => x.Parent)
                .Include(x => x.Booking)
                    .ThenInclude(x => x.TrialClass)
                .OrderByDescending(x => x.AttemptDate)
                .ThenByDescending(x => x.PaymentAttemptId)
                .ToListAsync();
        }

        public async Task<PaymentAttempt?> GetByIdAsync(Guid id)
        {
            return await _context.PaymentAttempts
                .FirstOrDefaultAsync(x => x.PaymentAttemptId == id);
        }

        public async Task<List<PaymentAttempt>> GetByBookingIdAsync(Guid bookingId)
        {
            return await _context.PaymentAttempts
                .Where(x => x.BookingId == bookingId)
                .ToListAsync();
        }

        public async Task AddAsync(PaymentAttempt paymentAttempt)
        {
            await _context.PaymentAttempts.AddAsync(paymentAttempt);
        }

        public void Update(PaymentAttempt paymentAttempt)
        {
            _context.PaymentAttempts.Update(paymentAttempt);
        }

        public void Delete(PaymentAttempt paymentAttempt)
        {
            _context.PaymentAttempts.Remove(paymentAttempt);
        }
    }
}

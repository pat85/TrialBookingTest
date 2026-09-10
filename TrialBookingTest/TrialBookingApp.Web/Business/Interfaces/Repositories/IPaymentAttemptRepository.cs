using System;
using System.Collections.Generic;
using System.Text;
using TrialBookingApp.Web.Domain.Entities;

namespace TrialBookingApp.Web.Business.Interfaces.Repositories
{
    public interface IPaymentAttemptRepository
    {
        Task<List<PaymentAttempt>> GetAllAsync();

        Task<PaymentAttempt?> GetByIdAsync(Guid id);

        Task<List<PaymentAttempt>> GetByBookingIdAsync(
            Guid bookingId);

        Task AddAsync(PaymentAttempt paymentAttempt);

        void Update(PaymentAttempt paymentAttempt);

        void Delete(PaymentAttempt paymentAttempt);
    }
}

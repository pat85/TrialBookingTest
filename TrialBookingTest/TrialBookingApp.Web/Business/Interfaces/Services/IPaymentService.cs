using System;
using System.Collections.Generic;
using System.Text;
using TrialBookingApp.Web.Models;
using TrialBookingApp.Web.Domain.Enums;

namespace TrialBookingApp.Web.Business.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<List<PaymentDetailsViewModel>> GetAllPaymentDetailsAsync();

        Task<PaymentStatus> ProcessPaymentAsync(decimal amount, bool canComplete);
    }
}

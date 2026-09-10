using System;
using System.Collections.Generic;
using System.Text;

namespace TrialBookingApp.Web.Domain.Enums
{
    public enum BookingStatus
    {
        PendingPayment,
        Confirmed,
        PaymentFailed,
        Cancelled
    }
}

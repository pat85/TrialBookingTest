using System;
using System.Collections.Generic;
using System.Text;
using TrialBookingApp.Web.Domain.Entities;

namespace TrialBookingApp.Web.Business.Interfaces.Services
{
    public interface IParentService
    {
        Task<List<Parent>> GetAllParentsAsync();
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using TrialBookingApp.Web.Business.Interfaces.Repositories;
using TrialBookingApp.Web.Business.Interfaces.Services;
using TrialBookingApp.Web.Domain.Entities;

namespace TrialBookingApp.Web.Business.Services
{
    public class ParentService : IParentService
    {
        private readonly IParentRepository _parentRepository;

        public ParentService(IParentRepository parentRepository)
        {
            _parentRepository = parentRepository;
        }

        public async Task<List<Parent>> GetAllParentsAsync()
        {
            return await _parentRepository.GetAllAsync();
        }
    }
}

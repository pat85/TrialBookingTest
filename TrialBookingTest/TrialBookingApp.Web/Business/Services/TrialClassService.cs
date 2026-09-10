using TrialBookingApp.Web.Business.Interfaces.Repositories;
using TrialBookingApp.Web.Business.Interfaces.Services;
using TrialBookingApp.Web.Domain.Entities;

namespace TrialBookingApp.Web.Business.Services
{
    public class TrialClassService : ITrialClassService
    {
        private readonly ITrialClassRepository _trialClassRepository;

        public TrialClassService(ITrialClassRepository trialClassRepository)
        {
            _trialClassRepository = trialClassRepository;
        }

        public async Task<List<TrialClass>> GetAllTrialClassesAsync()
        {
            return await _trialClassRepository.GetAllAsync();
        }
    }
}

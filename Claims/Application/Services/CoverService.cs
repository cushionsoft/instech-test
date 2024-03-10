using Claims.Core.Entities;
using Claims.Core.Repositories;
using Claims.Core.Services;

namespace Claims.Application.Services
{
    public class CoverService : ICoverService
    {
        private readonly IAuditRepository _auditRepository;
        private readonly ICoverRepository _coverRepository;

        public CoverService(IAuditRepository auditRepository, ICoverRepository coverRepository)
        {
            _auditRepository = auditRepository;
            _coverRepository = coverRepository;
        }

        public async Task<IEnumerable<Cover>> GetAsync()
        {
            return await _coverRepository.GetCoversAsync();
        }

        public async Task<Cover?> GetAsync(string id)
        {
            return await _coverRepository.GetCoverAsync(id);
        }

        public async Task<Cover> CreateAsync(Cover cover)
        {
            cover.Id = Guid.NewGuid().ToString();
            cover.Premium = ComputePremium(cover.StartDate, cover.EndDate, cover.Type);
            await _coverRepository.AddItemAsync(cover);
            _auditRepository.AuditCover(cover.Id, "POST");
            return cover;
        }

        public async Task DeleteAsync(string id)
        {
            _auditRepository.AuditCover(id, "DELETE");
            await _coverRepository.DeleteItemAsync(id);
        }

        public decimal ComputePremium(DateOnly startDate, DateOnly endDate, Core.Enums.CoverType coverType)
        {
            var multiplier = 1.3m;
            if (coverType == Core.Enums.CoverType.Yacht)
            {
                multiplier = 1.1m;
            }

            if (coverType == Core.Enums.CoverType.PassengerShip)
            {
                multiplier = 1.2m;
            }

            if (coverType == Core.Enums.CoverType.Tanker)
            {
                multiplier = 1.5m;
            }

            var premiumPerDay = 1250 * multiplier;
            var insuranceLength = endDate.DayNumber - startDate.DayNumber;
            var totalPremium = 0m;

            for (var i = 0; i < insuranceLength; i++)
            {
                if (i < 30) totalPremium += premiumPerDay;
                if (i < 180 && coverType == Core.Enums.CoverType.Yacht) totalPremium += premiumPerDay - premiumPerDay * 0.05m;
                else if (i < 180) totalPremium += premiumPerDay - premiumPerDay * 0.02m;
                if (i < 365 && coverType != Core.Enums.CoverType.Yacht) totalPremium += premiumPerDay - premiumPerDay * 0.03m;
                else if (i < 365) totalPremium += premiumPerDay - premiumPerDay * 0.08m;
            }

            return totalPremium;
        }
    }
}

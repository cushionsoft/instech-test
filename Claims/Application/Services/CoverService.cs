using Claims.Core.Entities;
using Claims.Core.Repositories;
using Claims.Core.Services;

namespace Claims.Application.Services
{
    public class CoverService : ICoverService
    {
        private readonly IAuditService _auditService;
        private readonly ICoverRepository _coverRepository;
        private readonly IPremiumCalculatorService _premiumCalculatorService;

        public CoverService(IAuditService auditService, IPremiumCalculatorService premiumCalculatorService, ICoverRepository coverRepository)
        {
            _auditService = auditService;
            _coverRepository = coverRepository;
            _premiumCalculatorService = premiumCalculatorService;
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
            cover.Premium = _premiumCalculatorService.ComputePremium(cover.StartDate, cover.EndDate, cover.Type);
            await _coverRepository.AddItemAsync(cover);
            _auditService.AddCoverAudit(new CoverAudit { CoverId = cover.Id, HttpRequestType = "POST" });
            return cover;
        }

        public async Task DeleteAsync(string id)
        {
            _auditService.AddCoverAudit(new CoverAudit { CoverId = id, HttpRequestType = "DELETE" });
            await _coverRepository.DeleteItemAsync(id);
        }
    }
}

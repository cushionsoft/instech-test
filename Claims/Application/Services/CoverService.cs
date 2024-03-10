using Claims.Core.Entities;
using Claims.Core.Repositories;
using Claims.Core.Services;

namespace Claims.Application.Services
{
    public class CoverService : ICoverService
    {
        private readonly IAuditService _auditService;
        private readonly ICoverRepository _coverRepository;

        public CoverService(IAuditService auditService, ICoverRepository coverRepository)
        {
            _auditService = auditService;
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

using Claims.Core.Entities;
using Claims.Core.Repositories;
using Claims.Core.Services;

namespace Claims.Application.Services
{
    public class ClaimService : IClaimService
    {
        private readonly IAuditService _auditService;
        private readonly IClaimRepository _claimRepository;

        public ClaimService(IAuditService auditService, IClaimRepository claimRepository)
        {
            _auditService = auditService;
            _claimRepository = claimRepository;
        }

        public Task<IEnumerable<Claim>> GetAsync()
        {
            return _claimRepository.GetClaimsAsync();
        }

        public async Task<Claim> CreateAsync(Claim claim)
        {
            claim.Id = Guid.NewGuid().ToString();
            await _claimRepository.AddItemAsync(claim);
            _auditService.AddClaimAudit(new ClaimAudit { ClaimId = claim.Id, HttpRequestType = "POST" });
            return claim;
        }

        public async Task DeleteAsync(string id)
        {
            _auditService.AddClaimAudit(new ClaimAudit { ClaimId = id, HttpRequestType = "DELETE" });
            await _claimRepository.DeleteItemAsync(id);
        }

        public async Task<Claim?> GetAsync(string id)
        {
            return await _claimRepository.GetClaimAsync(id);
        }
    }
}

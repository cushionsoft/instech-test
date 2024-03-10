using Claims.Core.Entities;
using Claims.Core.Repositories;
using Claims.Core.Services;

namespace Claims.Application.Services
{
    public class ClaimService : IClaimService
    {
        private readonly IAuditRepository _auditRepository;
        private readonly IClaimRepository _claimRepository;

        public ClaimService(IAuditRepository auditRepository, IClaimRepository claimRepository)
        {
            _auditRepository = auditRepository;
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
            _auditRepository.AuditClaim(claim.Id, "POST");
            return claim;
        }

        public async Task DeleteAsync(string id)
        {
            _auditRepository.AuditClaim(id, "DELETE");
            await _claimRepository.DeleteItemAsync(id);
        }

        public async Task<Claim?> GetAsync(string id)
        {
            return await _claimRepository.GetClaimAsync(id);
        }
    }
}

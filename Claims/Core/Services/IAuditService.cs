using Claims.Core.Entities;

namespace Claims.Core.Services
{
    public interface IAuditService
    {
        public void AddCoverAudit(CoverAudit coverAudit);

        public void AddClaimAudit(ClaimAudit claimAudit);
    }
}

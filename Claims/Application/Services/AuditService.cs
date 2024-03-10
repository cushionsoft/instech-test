using Claims.Core.Entities;
using Claims.Core.Repositories;
using Claims.Core.Services;
using System.Collections.Concurrent;

namespace Claims.Application.Services
{
    public class AuditService : IAuditService
    {
        private readonly BlockingCollection<ClaimAudit> _claimAudits;
        private readonly BlockingCollection<CoverAudit> _coverAudits;
        private readonly IServiceProvider _serviceProvider;

        public AuditService(IServiceProvider serviceProvider)
        {
            _claimAudits = new BlockingCollection<ClaimAudit>();
            _coverAudits = new BlockingCollection<CoverAudit>();
            _serviceProvider = serviceProvider;
            Task.Run(SaveClaims);
            Task.Run(SaveCovers);
        }

        public void AddCoverAudit(CoverAudit coverAudit)
        {
            _coverAudits.Add(coverAudit);
        }

        public void AddClaimAudit(ClaimAudit claimAudit)
        {
            _claimAudits.Add(claimAudit);
        }

        private async Task SaveClaims()
        {
            while (true)
            {
                var claimAudit = _claimAudits.Take();

                using (var scope = _serviceProvider.CreateScope())
                {
                    var auditRepository = scope.ServiceProvider.GetRequiredService<IAuditRepository>();
                    await auditRepository.AuditClaim(claimAudit.ClaimId, claimAudit.HttpRequestType);
                }
            }

        }

        private async Task SaveCovers()
        {
            while (true)
            {
                var coverAudt = _coverAudits.Take();
                using (var scope = _serviceProvider.CreateScope())
                {
                    var auditRepository = scope.ServiceProvider.GetRequiredService<IAuditRepository>();
                    await auditRepository.AuditCover(coverAudt.CoverId, coverAudt.HttpRequestType);
                }
            }

        }
    }
}

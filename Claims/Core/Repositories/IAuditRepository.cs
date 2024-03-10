namespace Claims.Core.Repositories
{
    public interface IAuditRepository
    {
        Task AuditClaim(string id, string httpRequestType);
        Task AuditCover(string id, string httpRequestType);
    }
}
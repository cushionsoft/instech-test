﻿namespace Claims.Core.Repositories
{
    public interface IAuditRepository
    {
        void AuditClaim(string id, string httpRequestType);
        void AuditCover(string id, string httpRequestType);
    }
}
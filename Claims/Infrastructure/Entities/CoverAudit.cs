﻿namespace Claims.Infrastructure.Entities
{
    public class CoverAudit
    {
        public int Id { get; set; }

        public required string CoverId { get; set; }

        public DateTime Created { get; set; }

        public required string HttpRequestType { get; set; }
    }
}

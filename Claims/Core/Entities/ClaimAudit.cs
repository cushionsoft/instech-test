namespace Claims.Core.Entities
{
    public class ClaimAudit
    {
        public int Id { get; set; }

        public required string ClaimId { get; set; }

        public DateTime Created { get; set; } = default!;

        public required string HttpRequestType { get; set; }
    }
}

using Claims.Core.Enums;

namespace Claims.Core.Entities
{
    public class Claim
    {
        public string Id { get; set; } = default!;

        public required string CoverId { get; set; }

        public required DateTime Created { get; set; }

        public required string Name { get; set; }

        public required ClaimType Type { get; set; }

        public required decimal DamageCost { get; set; }
    }
}

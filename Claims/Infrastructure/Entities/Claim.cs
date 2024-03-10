using Claims.Core.Enums;
using Newtonsoft.Json;

namespace Claims.Infrastructure.Entities
{
    public class Claim
    {
        [JsonProperty(PropertyName = "id")]
        public required string Id { get; set; } = default!;

        public required string CoverId { get; set; }

        public required DateTime Created { get; set; }

        public required string Name { get; set; }

        public required ClaimType Type { get; set; }

        public required decimal DamageCost { get; set; }
    }
}

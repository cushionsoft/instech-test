using Claims.Core.Enums;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Claims.Web.Models
{
    public class Claim
    {
        [SwaggerSchema(ReadOnly = true)]
        [JsonProperty(PropertyName = "id")]
        public string? Id { get; set; }

        [JsonProperty(PropertyName = "coverId")]
        public required string CoverId { get; set; }

        [JsonProperty(PropertyName = "created")]
        public required DateTime Created { get; set; }

        [JsonProperty(PropertyName = "name")]
        public required string Name { get; set; }

        [JsonProperty(PropertyName = "claimType")]
        public required ClaimType Type { get; set; }

        [JsonProperty(PropertyName = "damageCost")]
        public required decimal DamageCost { get; set; }

    }
}

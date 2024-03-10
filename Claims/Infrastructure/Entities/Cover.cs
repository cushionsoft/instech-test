using Claims.Core.Enums;
using Newtonsoft.Json;

namespace Claims.Infrastructure.Entities
{
    public class Cover
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = default!;

        public required DateOnly StartDate { get; set; }

        public required DateOnly EndDate { get; set; }

        public required CoverType Type { get; set; }

        public required decimal Premium { get; set; }
    }
}

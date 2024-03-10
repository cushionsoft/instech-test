using Claims.Core.Enums;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Claims.Web.Models;

public class Cover
{
    [SwaggerSchema(ReadOnly = true)]
    [JsonProperty(PropertyName = "id")]
    public string? Id { get; set; }

    [JsonProperty(PropertyName = "startDate")]
    public DateOnly StartDate { get; set; }

    [JsonProperty(PropertyName = "endDate")]
    public DateOnly EndDate { get; set; }

    [JsonProperty(PropertyName = "claimType")]
    public CoverType Type { get; set; }

    [JsonProperty(PropertyName = "premium")]
    public decimal Premium { get; set; }
}
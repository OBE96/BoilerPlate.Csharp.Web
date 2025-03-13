using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BoilerPlate.Domain.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SubscriptionPlan
    {
        Free,
        Basic,
        Advanced,
        Premium
    }
}

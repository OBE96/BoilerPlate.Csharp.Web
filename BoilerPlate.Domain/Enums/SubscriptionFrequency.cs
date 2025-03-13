using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace BoilerPlate.Domain.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SubscriptionFrequency
    {
        Monthly,
        Quarterly,
        HalfYearly,
        Annually
    }
}
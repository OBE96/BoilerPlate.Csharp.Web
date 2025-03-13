using Newtonsoft.Json;
using Newtonsoft.Json.Converters;



namespace BoilerPlate.Domain.Enums
{

    [JsonConverter(typeof(StringEnumConverter))]
    public enum TransactionStatus
    {
        Pending,
        Completed,
        Failed
    }
}
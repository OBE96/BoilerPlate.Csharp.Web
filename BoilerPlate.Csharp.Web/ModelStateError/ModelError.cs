using System.Text.Json.Serialization;

namespace BoilerPlate.Web.ModelStateError
{
    public record ModelError
    {
        [JsonPropertyName("field")]
        public string? Field { get; init; }


        [JsonPropertyName("message")]
        public string? Message { get; init; }
    }

}

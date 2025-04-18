using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BoilerPlate.Application.Shared.Dtos
{
    public class BaseQueryParameters
    {
        [JsonPropertyName("limit")]
        public int Limit { get; set; } = 10;
        [JsonPropertyName("offset")]
        public int Offset { get; set; } = 1;
    }
}

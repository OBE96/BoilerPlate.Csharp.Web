using BoilerPlate.Application.Shared.Dtos;
using System.Text.Json.Serialization;


namespace BoilerPlate.Application.Features.SuperAdmin.Dto
{
    public class UsersQueryParameters: BaseQueryParameters
    {
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("password")]
        public string? FirstName { get; set; }

        [JsonPropertyName("lastname")]
        public string? LastName { get; set; }
    }
}

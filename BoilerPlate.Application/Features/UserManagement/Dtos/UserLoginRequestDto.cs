
using System.Text.Json.Serialization;

namespace BoilerPlate.Application.Features.UserManagement.Dtos
{
    public class UserLoginRequestDto
    {
        [JsonPropertyName("email")]
        public string? Email { get; set; }
        [JsonPropertyName("password")]
        public string? Password { get; set; }
       // public string? Status {  get; set; }
    }
}

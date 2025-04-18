using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace BoilerPlate.Application.Features.UserManagement.Dtos
{
    public class GoogleLoginRequestDto
    {
        [Required]
        [JsonPropertyName("id_token")]
        public string IdToken { get; set; }
    }
}

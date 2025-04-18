using MediatR;
using CSharpFunctionalExtensions;
using System.Text.Json.Serialization;

namespace BoilerPlate.Application.Features.UserManagement.Dtos
{

    public record ForgotPasswordDto : ForgotPasswordRequestDto, IRequest<Result<ForgotPasswordResponse>>
    {
        public bool isMobile { get; set; }
        public ForgotPasswordDto(string email , bool isMobile)
        {
            Email = email;
            this.isMobile = isMobile;
        }
    }

    public record ForgotPasswordRequestDto

    {
        [JsonPropertyName("email")]
        public string? Email { get; set; }
    }

    public record ForgotPasswordResponse
    {
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; }

        [JsonPropertyName("data")]
        public ForgotPasswordData? Data { get; set; }
    }

    public record ForgotPasswordData
    {
        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }

}

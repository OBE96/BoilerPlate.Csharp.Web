using BoilerPlate.Application.Features.Organisations.Dtos;
using BoilerPlate.Application.Features.Products.Dtos;
using BoilerPlate.Application.Features.Profiles.Dtos;
using BoilerPlate.Domain.Entities;
using System.Text.Json.Serialization;


namespace BoilerPlate.Application.Features.UserManagement.Dtos
{
    public class UserDto
    {
        [JsonPropertyName("fullname")]
        public string? FullName { get; set; }

        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("profile")]
        public ProfileDto? Profile { get; set; }

        [JsonPropertyName("avatar_url")]
        public string? AvatarUrl { get; set; }

        [JsonPropertyName("organisations")]
        public IEnumerable<OrganizationDto>? Organizations { get; set; }

        [JsonIgnore]
        [JsonPropertyName("products")]
        public IEnumerable<ProductDto>? Products { get; set; }

        [JsonPropertyName("blogs")]
        public ICollection<Blog> Blogs { get; set; } = [];
    }
}
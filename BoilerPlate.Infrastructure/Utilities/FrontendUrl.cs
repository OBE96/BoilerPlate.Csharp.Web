using System.ComponentModel.DataAnnotations;

namespace BoilerPlate.Infrastructure.Utilities
{
    public class FrontendUrl
    {
        [Required(ErrorMessage = "Frontend URL path is required")]
        [Url(ErrorMessage = "Invalid URL format")]
        public string? Path { get; set; }
    }
}

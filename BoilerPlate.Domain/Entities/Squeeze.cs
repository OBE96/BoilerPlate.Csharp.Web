using System.ComponentModel.DataAnnotations;


namespace BoilerPlate.Domain.Entities
{
    public class Squeeze : EntityBase
    {
        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
    }
}

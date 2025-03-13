using System.ComponentModel.DataAnnotations;


namespace BoilerPlate.Domain.Entities
{
    //One Language to Many Users: one-to-many
    //A single Language can be associated with many User entities. For example,
    //one language (e.g., "English") can be spoken by many users.
    //This is represented by the ICollection<User> Users in the Language entity.
    public class Language : EntityBase
    {
        [Required]
        public string? Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}

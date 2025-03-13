using BoilerPlate.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BoilerPlate.Domain.Entities
{
    //one-to-many relationship:
    // One User can have many Blog: This is the main relationship between the two entities.
    // A user can own multiple Blogs, but each Blog is owned by exactly one user.
    public class Blog : EntityBase
    {
        [Required]
        public string? Title { get; set; }
        public string? ImageUrl { get; set; }
        [Required]
        public string? Content { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid AuthorId { get; set; }
        public User? Author { get; set; }
        public BlogCategory Category { get; set; }
        public ICollection<Comment>? Comments { get; set; }
    }
}

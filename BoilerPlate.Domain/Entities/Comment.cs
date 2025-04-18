using System.ComponentModel.DataAnnotations;

namespace BoilerPlate.Domain.Entities
{
    //one-to-many relationship:
    // One Blog can have many comments: This is the main relationship between the two entities.
    // A Blog can own multiple comments, but each comment is owned by exactly one blog.
    public class Comment : EntityBase
    {
        [Required]
        public string? Content { get; set; }
        public Guid BlogId { get; set; }
        public Blog? Blog { get; set; }
        public Guid AuthorId { get; set; }
        public User? Author { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
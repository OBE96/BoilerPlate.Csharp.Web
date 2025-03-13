namespace BoilerPlate.Domain.Entities
{
    //one-to-many relationship:
    // One User can have many LastLogin: This is the main relationship between the two entities.
    // A user can own multiple Lastlogin, but each lastlogin is owned by exactly one user.
    public class LastLogin : EntityBase
    {
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public DateTime LoginTime { get; set; } = DateTime.UtcNow;
        public DateTime? LogoutTime { get; set; }
        public string? IPAddress { get; set; }
    }
}

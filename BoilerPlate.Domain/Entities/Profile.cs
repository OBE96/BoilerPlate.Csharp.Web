namespace BoilerPlate.Domain.Entities
{
    //one-to-one relationship 
    //The Profile class has a one-to-one relationship with the User class because each profile is
    //associated with exactly one user, but one user can have one profile

    public class Profile : EntityBase
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AvatarUrl { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public string? Username { get; set; }
        public string? Pronoun { get; set; }
        public string? JobTitle { get; set; }
        public string? Bio { get; set; }
        public string? Department { get; set; }
        public string? FacebookLink { get; set; }
        public string? TwitterLink { get; set; }
        public string? LinkedinLink { get; set; }
        public string? InstagramLink { get; set; }
    }
}

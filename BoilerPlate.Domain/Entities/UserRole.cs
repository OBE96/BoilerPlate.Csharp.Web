namespace BoilerPlate.Domain.Entities
{
    //one-to-many relationship:
    // One User can have many Roles: This is the main relationship between the two entities.
    // A user can own multiple Roles, but each Roles is owned by exactly one user.
    // A userRole can own multiple Roles, but each Role is owned by exactly one userRole.
    public class UserRole:EntityBase
    {
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public Guid RoleId { get; set; }
        public Role? Role { get; set; }
        public Guid OrganizationId { get; set; } 
        public Organization? Orgainzation { get; set; }
    }
}

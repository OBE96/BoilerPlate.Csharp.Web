namespace BoilerPlate.Domain.Entities
{
    //many-to-many relationship
    //A single Role can have many RolePermission.
    //A single RolePermission  can belong to many Roles
    public class RolePermission : EntityBase
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = null;
        public ICollection<Role> Roles { get; set; } = [];
    }
}

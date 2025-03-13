﻿namespace BoilerPlate.Domain.Entities
{
    //many-to-many relationship
    //A single Organization can have many Users (the Users property in the Organization class).
    //A single User can belong to many Organizations (the Organizations property in the User class).
    public class Organization : EntityBase
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Slug { get; set; }

        public string? Email { get; set; }

        public string? Industry { get; set; }

        public string? Type { get; set; }

        public string? Country { get; set; }

        public string? Address { get; set; }

        public string? State { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Guid OwnerId { get; set; }

        public bool IsActive { get; set; }

        public Guid InviteToken { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<UserRole> UsersRoles { get; set; } = new List<UserRole>();

        public ICollection<Subscription> Subscriptions { get; set; } = [];
    }
}

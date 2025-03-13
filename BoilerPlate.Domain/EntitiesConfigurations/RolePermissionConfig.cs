using BoilerPlate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace BoilerPlate.Domain.EntitiesConfigurations
{
    public class RolePermissionConfig : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name)
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(x => x.Description);
            builder.Property(x => x.CreatedAt).IsRequired();

             //many - to - many relationship
            //A single Role can have many RolePermission.
            //A single RolePermission  can belong to many Roles
            builder.HasMany(rp => rp.Roles)
                .WithMany(r => r.Permissions);
        }
    }
}

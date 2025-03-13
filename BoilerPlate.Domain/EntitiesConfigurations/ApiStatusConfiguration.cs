using BoilerPlate.Domain.Entities;
using BoilerPlate.Domain.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;

namespace BoilerPlate.Domain.EntitiesConfigurations
{
    public class ApiStatusConfiguration : IEntityTypeConfiguration<ApiStatus>
    {
        public void Configure(EntityTypeBuilder<ApiStatus> builder)
        {
            // This specifies that the Id property of the ApiStatus entity is the primary key in the database table.
            builder
                .HasKey(i => i.Id);


            //EnumToStringConverter<ApiStatusType>: This converts the enum value to a string when saving to
            //the database and back to an enum when retrieving from the database. This is useful because databases
            //typically don’t support enums directly.

            builder
                .Property(p => p.Status)
                .HasConversion(new EnumToStringConverter<ApiStatusType>());

            //The ApiGroup property is configured to have a maximum length of
            //100 characters in the database. This ensures that the database
            //column for ApiGroup will not accept values longer than 100 characters.

            builder
                .Property(a => a.ApiGroup)
                .HasMaxLength(100);
        }
    }
}
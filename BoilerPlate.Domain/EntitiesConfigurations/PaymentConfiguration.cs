using BoilerPlate.Domain.Enums;
using BoilerPlate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;


namespace BoilerPlate.Domain.EntitiesConfigurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            //EnumToStringConverter<TransactionType>: This converts the enum value to a string when saving to
            //the database and back to an enum when retrieving from the database. This is useful because databases
            //typically don’t support enums directly.
            builder.Property(p => p.Type).HasConversion(new EnumToStringConverter<TransactionType>());
            builder.Property(p => p.Status).HasConversion(new EnumToStringConverter<TransactionStatus>());
            builder.Property(p => p.Partners).HasConversion(new EnumToStringConverter<TransactionIntegrationPartners>());

            //When you call HasIndex(), you're telling EF Core to create
            //an index on the specified property or properties in the database.

            builder.HasIndex(r => r.Reference);
        }
    }
}

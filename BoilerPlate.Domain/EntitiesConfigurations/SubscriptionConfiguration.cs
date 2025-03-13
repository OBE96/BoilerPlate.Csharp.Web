using BoilerPlate.Domain.Entities;
using BoilerPlate.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BoilerPlate.Domain.EntitiesConfigurations
{
    public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.Property(p => p.Plan).HasConversion(new EnumToStringConverter<SubscriptionPlan>());
            builder.Property(p => p.Frequency).HasConversion(new EnumToStringConverter<SubscriptionFrequency>());
        }
    }
}

using BoilerPlate.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace BoilerPlate.Domain.EntitiesConfigurations
{
    public class NewsLetterSubscriberConfig : IEntityTypeConfiguration<NewsLetterSubscriber>
    {
        public void Configure(EntityTypeBuilder<NewsLetterSubscriber> builder)
        {
            builder.HasKey(nl => nl.Id);
            builder.HasIndex(nl => nl.Email)
                .IsUnique();
            builder.Property(nl => nl.Email)
                .HasMaxLength(150)
                .IsRequired();
        }
    }
}

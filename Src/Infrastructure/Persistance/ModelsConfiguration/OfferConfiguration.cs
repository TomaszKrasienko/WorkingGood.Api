using Domain.Models.Employee;
using Domain.Models.Offer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.ModelsConfiguration;

public class OfferConfiguration : IEntityTypeConfiguration<Offer>
{
    public void Configure(EntityTypeBuilder<Offer> builder)
    {
        builder
            .Property(x => x.Id)
            .IsRequired();
        builder
            .OwnsOne(x => x.Content,
                options =>
                {

                    options
                        .Property(x => x.Title)
                        .IsRequired()
                        .HasMaxLength(200);
                    options
                        .Property(x => x.Description)
                        .IsRequired();
                });
        // builder
        //     .Property(x => x.Title)
        //     .IsRequired()
        //     .HasMaxLength(200);
        // builder
        //     .Property(x => x.Description)
        //     .IsRequired();
        builder
            .Property(x => x.AuthorId)
            .IsRequired();
        builder
            .HasKey(x => x.Id);
        builder
            .OwnsOne(x => x.SalaryRanges,
                options =>
                {
                    options
                        .Property(x => x.ValueMin);
                    options
                        .Property(x => x.ValueMax);
                });
        builder.OwnsOne(x => x.OfferStatus,
            options =>
            {
                options
                    .Property(x => x.IsActive)
                    .IsRequired();
            });
        builder
            .HasOne(x => x.Position);
        builder
            .HasOne<Employee>()
            .WithMany()
            .HasForeignKey(x => x.AuthorId);
    }
}
using Domain.Models.Offer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.ModelsConfiguration;

public class PositionConfiguration : IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        builder
            .Property(x => x.Id)
            .IsRequired();
        builder
            .Property(x => x.Type)
            .IsRequired();
        builder
            .HasKey(x => x.Id);
    }
}
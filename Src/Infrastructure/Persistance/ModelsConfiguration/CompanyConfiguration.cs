using System;
using Domain.Models.Company;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistance.ModelsConfiguration
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder
                .Property(x => x.Id)
                .IsRequired();
            builder
                .OwnsOne(x => x.Name,
                    options =>
                    {
                        options
                            .Property(x => x.Name)
                            .IsRequired()
                            .HasMaxLength(200);
                    });
            builder.OwnsOne(x => x.Logo,
                options =>
                {
                    options
                        .Property(x => x.Logo);
                });
            builder
                .HasKey(x => x.Id);
        }
    }
}


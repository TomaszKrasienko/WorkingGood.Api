using System;
using Domain.Models.Company;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
                .Property(x => x.Name)
                .IsRequired();
            builder
                .HasKey(x => x.Id);
        }
    }
}


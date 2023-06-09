﻿using System;
using Domain.Models.Company;
using Domain.Models.Employee;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.ModelsConfiguration
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder
                .Property(x => x.Id)
                .IsRequired();
            builder.OwnsOne(x => x.EmployeeName,
                options =>
                {
                    options
                        .Property(x => x.FirstName)
                        .IsRequired()
                        .HasMaxLength(60);
                    options
                        .Property(x => x.LastName)
                        .IsRequired()
                        .HasMaxLength(60);
                });
            // builder
            //     .Property(x => x.FirstName)
            //     .IsRequired()
            //     .HasMaxLength(60);
            // builder
            //     .Property(x => x.LastName)
            //     .IsRequired()
            //     .HasMaxLength(60);
            builder.OwnsOne(x => x.Email, options =>
            {
                options
                    .Property(x => x.EmailAddress)
                    .IsRequired()
                    .HasMaxLength(60);
            });
            // builder
            //     .Property(x => x.Email)
            //     .IsRequired()
            //     .HasMaxLength(60);
            builder
                .OwnsOne(x => x.Password,
                    options =>
                    {
                        options
                            .Property(x => x.Hash)
                            .IsRequired();
                        options
                            .Property(x => x.Salt)
                            .IsRequired();
                    });
            builder
                .OwnsOne(x => x.RefreshToken,
                    options =>
                    {
                        options
                            .Property(x => x.CreatedAt);
                        options
                            .Property(x => x.Token);
                        options
                            .Property(x => x.Expiration);
                    });
            builder
                .OwnsOne(x => x.EmployeeStatus,
                    options =>
                    {
                        options
                            .Property(x => x.IsActive);
                    });
            // builder
            //     .Property(x => x.IsActive);
            builder
                .OwnsOne(x => x.VerificationToken,
                    options =>
                    {
                        options
                            .Property(x => x.Token);
                        options
                            .Property(x => x.ConfirmDate);
                    });
            builder
                .OwnsOne(x => x.ResetToken,
                    options =>
                    {
                        options
                            .Property(x => x.Token);
                        options
                            .Property(x => x.ExpirationDate);
                    });
            builder
                .Property(x => x.CompanyId)
                .IsRequired();
            builder
                .HasKey(x => x.Id);
            builder
                .HasOne<Company>()
                .WithMany()
                .HasForeignKey(x => x.CompanyId);
        }
    }
}


﻿// <auto-generated />
using System;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Persistance.Migrations
{
    [DbContext(typeof(WgDbContext))]
    partial class WgDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Domain.Models.Company.Company", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("Domain.Models.Company.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Domain.Models.Company.Employee", b =>
                {
                    b.HasOne("Domain.Models.Company.Company", null)
                        .WithMany("Employees")
                        .HasForeignKey("CompanyId");

                    b.OwnsOne("Domain.ValueObjects.Password", "Password", b1 =>
                        {
                            b1.Property<Guid>("EmployeeId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<byte[]>("Hash")
                                .IsRequired()
                                .HasColumnType("varbinary(max)");

                            b1.Property<byte[]>("Salt")
                                .IsRequired()
                                .HasColumnType("varbinary(max)");

                            b1.HasKey("EmployeeId");

                            b1.ToTable("Employees");

                            b1.WithOwner()
                                .HasForeignKey("EmployeeId");
                        });

                    b.OwnsOne("Domain.ValueObjects.RefreshToken", "RefreshToken", b1 =>
                        {
                            b1.Property<Guid>("EmployeeId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<DateTime?>("CreatedAt")
                                .HasColumnType("datetime2");

                            b1.Property<DateTime?>("Expiration")
                                .HasColumnType("datetime2");

                            b1.Property<string>("Token")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("EmployeeId");

                            b1.ToTable("Employees");

                            b1.WithOwner()
                                .HasForeignKey("EmployeeId");
                        });

                    b.OwnsOne("Domain.ValueObjects.ResetToken", "ResetToken", b1 =>
                        {
                            b1.Property<Guid>("EmployeeId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<DateTime?>("ExpirationDate")
                                .HasColumnType("datetime2");

                            b1.Property<string>("Token")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("EmployeeId");

                            b1.ToTable("Employees");

                            b1.WithOwner()
                                .HasForeignKey("EmployeeId");
                        });

                    b.OwnsOne("Domain.ValueObjects.VerificationToken", "VerificationToken", b1 =>
                        {
                            b1.Property<Guid>("EmployeeId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<DateTime?>("ConfirmDate")
                                .HasColumnType("datetime2");

                            b1.Property<string>("Token")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("EmployeeId");

                            b1.ToTable("Employees");

                            b1.WithOwner()
                                .HasForeignKey("EmployeeId");
                        });

                    b.Navigation("Password")
                        .IsRequired();

                    b.Navigation("RefreshToken")
                        .IsRequired();

                    b.Navigation("ResetToken")
                        .IsRequired();

                    b.Navigation("VerificationToken")
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Models.Company.Company", b =>
                {
                    b.Navigation("Employees");
                });
#pragma warning restore 612, 618
        }
    }
}

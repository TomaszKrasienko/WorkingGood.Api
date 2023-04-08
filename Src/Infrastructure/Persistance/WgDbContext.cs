using System;
using Domain.Models.Company;
using Domain.Models.Employee;
using Domain.Models.Offer;
using Infrastructure.Persistance.ModelsConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance
{
	public class WgDbContext : DbContext
	{
        private string conn = "";
		public DbSet<Company> Companies { get; set; }
		public DbSet<Employee> Employees { get; set; }
		public DbSet<Offer> Offers { get; set; }
		public DbSet<Position> Positions { get; set; }
		public WgDbContext()
		{
		}
		public WgDbContext(DbContextOptions<WgDbContext> options) : base(options)
		{

		}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			modelBuilder.ApplyConfiguration(new CompanyConfiguration());
			modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
			modelBuilder.ApplyConfiguration(new OfferConfiguration());
			modelBuilder.ApplyConfiguration(new PositionConfiguration());
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}


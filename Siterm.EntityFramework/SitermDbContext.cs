using Microsoft.EntityFrameworkCore;
using Siterm.Domain.Models;

namespace Siterm.EntityFramework
{
    public class SitermDbContext : DbContext
    {
        public SitermDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Instruction> Instructions { get; set; }
        public DbSet<ServiceReport> ServiceReports { get; set; }
        public DbSet<Substance> Substances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Facility>().HasMany(f => f.Devices).WithOne(d => d.Facility).IsRequired();

            modelBuilder.Entity<Device>().HasMany<Instruction>(d => d.Instructions).WithOne(i => i.Device).IsRequired();
            modelBuilder.Entity<Device>().HasMany<ServiceReport>(d => d.ServiceReports).WithOne(s => s.Device).IsRequired();

            modelBuilder.Entity<User>().HasMany(u => u.PerformedInstructions).WithOne(i => i.Instructor);
            modelBuilder.Entity<User>().HasMany(u => u.Instructions).WithOne(i => i.Instructed);
            base.OnModelCreating(modelBuilder);
        }
    }
}
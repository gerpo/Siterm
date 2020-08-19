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
            modelBuilder.Entity<Facility>().HasMany(f => f.Devices).WithOne(d => d.Facility).IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Device>().HasMany(d => d.Instructions).WithOne(i => i.Device).IsRequired()
                .HasForeignKey(i => i.DeviceId).OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Device>().HasMany(d => d.ServiceReports).WithOne(s => s.Device).IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>().HasMany(u => u.PerformedInstructions).WithOne(i => i.Instructor)
                .HasForeignKey(i => i.InstructorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>().HasMany(u => u.Instructions).WithOne(i => i.Instructed)
                .HasForeignKey(i => i.InstructedId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Instruction>().HasQueryFilter(i => !i.IsArchived);
            base.OnModelCreating(modelBuilder);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Siterm.Domain.Models;

namespace Siterm.EntityFramework
{
    public class SitermDbContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Instruction> Instructions { get; set; }
        public DbSet<ServiceReport> ServiceReports { get; set; }
        public DbSet<Substance> Substances { get; set; }

        public SitermDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasMany(u => u.PerformedInstructions).WithOne(i => i.Instructor);
            modelBuilder.Entity<User>().HasMany(u => u.Instructions).WithOne(i => i.Instructed);
            base.OnModelCreating(modelBuilder);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Siterm.EntityFramework
{
    public class SitermDbContextFactory: IDesignTimeDbContextFactory<SitermDbContext>
    {
        public SitermDbContext CreateDbContext(string[] args = null)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SitermDbContext>();
            optionsBuilder.UseMySQL("server=localhost;database=siterm;user=root;password=root");

            return new SitermDbContext(optionsBuilder.Options);
        }

    }
}

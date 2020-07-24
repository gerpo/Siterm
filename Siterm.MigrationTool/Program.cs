using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Siterm.EntityFramework;

namespace Siterm.MigrationTool
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.ReadLine();

            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true);

            var configuration = configurationBuilder.Build();
            var connectionString = configuration.GetConnectionString("Database");

            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine("Connection String in appsettings.json not found.");
                Console.ReadLine();
            }

            var optionsBuilder = new DbContextOptionsBuilder<SitermDbContext>()
                .UseMySQL(connectionString);

            Console.Read();
        }
    }
}

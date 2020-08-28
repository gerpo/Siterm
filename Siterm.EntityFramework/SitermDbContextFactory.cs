using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Siterm.Settings.Models;
using Siterm.Settings.Services;

namespace Siterm.EntityFramework
{
    public class SitermDbContextFactory : IDesignTimeDbContextFactory<SitermDbContext>
    {
        private readonly SettingsProvider _settingsProvider;

        public SitermDbContextFactory(SettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
        }
        public SitermDbContext CreateDbContext(string[] args = null)
        {
            var connectionString = _settingsProvider.GetSetting(SettingName.DatabaseConnectionString).Value;
            var optionsBuilder = new DbContextOptionsBuilder<SitermDbContext>();
            optionsBuilder
                .UseMySQL(connectionString);
                //.UseMySQL("server=localhost;database=siterm;user=root;password=root");
            //.UseLazyLoadingProxies();

            return new SitermDbContext(optionsBuilder.Options);
        }
    }
}
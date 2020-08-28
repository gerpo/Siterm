using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Siterm.Domain.Models;

namespace Siterm.EntityFramework.Services
{
    public class DeviceDataService : GenericDataService<Device>
    {
        public DeviceDataService(SitermDbContextFactory contextFactory) : base(contextFactory)
        {
        }

        public async Task<Device> GetFull(int id)
        {
            await using var context = ContextFactory.CreateDbContext();

            return await context.Set<Device>()
                .Include(d => d.Chief)
                .Include(d => d.ServiceReports)
                .Include(d => d.Instructions)
                .ThenInclude(i => i.Instructed)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Device> FirstFullOrDefault(Expression<Func<Device, bool>> predicate)
        {
            await using var context = ContextFactory.CreateDbContext();

            return await context.Set<Device>()
                .Include(d => d.Chief)
                .Include(d => d.ServiceReports)
                .Include(d => d.Instructions)
                .FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<string>> GetNames()
        {
            await using var context = ContextFactory.CreateDbContext();

            return await context.Set<Device>().Select(d => d.Name).ToListAsync();
        }
    }
}
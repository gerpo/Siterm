﻿using System.Threading.Tasks;
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
                .Include(d => d.ServiceReports)
                .Include(d => d.Instructions)
                .ThenInclude(i => i.Instructed)
                .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
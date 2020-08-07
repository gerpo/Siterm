using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Siterm.Domain.Models;

namespace Siterm.EntityFramework.Services
{
    public class FacilityDataService : GenericDataService<Facility>
    {
        public FacilityDataService(SitermDbContextFactory contextFactory) : base(contextFactory)
        {
        }

        public async Task<IEnumerable<Facility>> GetAllWithDevices()
        {
            await using var context = ContextFactory.CreateDbContext();

            return await context.Set<Facility>().Include(f => f.Devices).ToListAsync();
        }
    }
}
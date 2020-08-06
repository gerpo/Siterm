using Siterm.Domain.Models;

namespace Siterm.EntityFramework.Services
{
    public class FacilityDataService : GenericDataService<Facility>
    {
        public FacilityDataService(SitermDbContextFactory contextFactory) : base(contextFactory)
        {
        }
    }
}
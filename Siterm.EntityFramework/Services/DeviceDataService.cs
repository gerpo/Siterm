using Siterm.Domain.Models;

namespace Siterm.EntityFramework.Services
{
    public class DeviceDataService : GenericDataService<Device>
    {
        public DeviceDataService(SitermDbContextFactory contextFactory) : base(contextFactory)
        {
        }
    }
}
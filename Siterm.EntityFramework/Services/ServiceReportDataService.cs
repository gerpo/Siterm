using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Siterm.Domain.Models;

namespace Siterm.EntityFramework.Services
{
    public class ServiceReportDataService : GenericDataService<ServiceReport>
    {
        public ServiceReportDataService(SitermDbContextFactory contextFactory) : base(contextFactory)
        {
        }

        public async Task<IEnumerable<ServiceReport>> GetAllForMailNotification()
        {
            await using var context = ContextFactory.CreateDbContext();

            return (await context.Set<ServiceReport>()
                .Include(s => s.Device)
                .ThenInclude(d => d.Chief)
                .Where(s => !s.NotificationSent)
                .ToListAsync()).Where(s => s.HasWarning || s.IsInvalid);
        }

        public async Task<ServiceReport> MarkAsNotified(ServiceReport serviceReport)
        {
            serviceReport.NotificationSent = true;
            return await Update(serviceReport.Id, serviceReport);
        }

        public async Task<ServiceReport> MarkAsNotNotified(ServiceReport serviceReport)
        {
            serviceReport.NotificationSent = false;
            return await Update(serviceReport.Id, serviceReport);
        }
    }
}
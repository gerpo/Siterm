using Siterm.Domain.Models;

namespace Siterm.EntityFramework.Services
{
    public class SubstanceDataService : GenericDataService<Substance>
    {
        public SubstanceDataService(SitermDbContextFactory contextFactory) : base(contextFactory)
        {
        }
    }
}
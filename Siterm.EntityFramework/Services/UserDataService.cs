using Siterm.Domain.Models;

namespace Siterm.EntityFramework.Services
{
    public class UserDataService : GenericDataService<User>

    {
        public UserDataService(SitermDbContextFactory contextFactory) : base(contextFactory)
        {
        }
    }
}
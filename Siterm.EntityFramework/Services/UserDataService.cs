using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Siterm.Domain.Models;

namespace Siterm.EntityFramework.Services
{
    public class UserDataService : GenericDataService<User>

    {
        public UserDataService(SitermDbContextFactory contextFactory) : base(contextFactory)
        {
        }

        public async Task<IEnumerable<string>> GetAllEmails()
        {
            await using var context = ContextFactory.CreateDbContext();

            return await context.Set<User>().Select(u => u.Email).ToListAsync();
        }
    }
}
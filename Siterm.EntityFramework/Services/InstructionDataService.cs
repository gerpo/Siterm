using Siterm.Domain.Models;

namespace Siterm.EntityFramework.Services
{
    public class InstructionDataService : GenericDataService<Instruction>
    {
        public InstructionDataService(SitermDbContextFactory contextFactory) : base(contextFactory)
        {
        }
    }
}
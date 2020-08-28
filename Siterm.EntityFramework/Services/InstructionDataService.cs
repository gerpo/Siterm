using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Siterm.Domain.Models;

namespace Siterm.EntityFramework.Services
{
    public class InstructionDataService : GenericDataService<Instruction>
    {
        public InstructionDataService(SitermDbContextFactory contextFactory) : base(contextFactory)
        {
        }

        public async Task<IEnumerable<Instruction>> GetAllForMailNotification()
        {
            await using var context = ContextFactory.CreateDbContext();

            return (await context.Set<Instruction>()
                .Include(i => i.Instructed)
                .Include(i => i.Device)
                .ThenInclude(d => d.Chief)
                .Where(i => !i.NotificationSent)
                .ToListAsync()).Where(i => i.HasWarning || i.IsInvalid);
        }

        public async Task<Instruction> MarkAsNotified(Instruction instruction)
        {
            instruction.NotificationSent = true;
            return await Update(instruction.Id, instruction);
        }

        public async Task<Instruction> MarkAsNotNotified(Instruction instruction)
        {
            instruction.NotificationSent = false;
            return await Update(instruction.Id, instruction);
        }
    }
}
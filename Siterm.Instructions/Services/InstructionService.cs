using Siterm.EntityFramework.Services;
using Siterm.Instructions.Models;

namespace Siterm.Instructions.Services
{
    public class InstructionService
    {
        private readonly InstructionDataService _instructionDataService;

        public InstructionService(InstructionDataService instructionDataService)
        {
            _instructionDataService = instructionDataService;
        }

        public void CreateInstruction(InstructionDraft instructionDraft)
        {

        }
    }
}
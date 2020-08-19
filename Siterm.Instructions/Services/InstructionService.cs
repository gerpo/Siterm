using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Siterm.Domain.Models;
using Siterm.EntityFramework.Services;
using Siterm.Instructions.Models;

namespace Siterm.Instructions.Services
{
    public class InstructionService
    {
        private readonly InstructionDataService _instructionDataService;
        private readonly InstructionPdfService _instructionPdfService;
        private readonly UserDataService _userDataService;

        public InstructionService(InstructionDataService instructionDataService,
            InstructionPdfService instructionPdfService, UserDataService userDataService)
        {
            _instructionDataService = instructionDataService;
            _instructionPdfService = instructionPdfService;
            _userDataService = userDataService;
        }

        public async void CreateInstruction(InstructionDraft instructionDraft)
        {
            var pdfCreated = _instructionPdfService.CreateInstructionPdf(instructionDraft);
            var instructions = new List<Instruction>();
            var instructor = await GetUser(instructionDraft.Instructor);

            for (var i = 0; i < instructionDraft.Instructed.Count; i++)
            {
                var instructionPath = pdfCreated[i];
                if (instructionPath is null) continue;

                var userDraft = instructionDraft.Instructed[i];
                var user = await GetUser(userDraft);

                var instruction = new Instruction
                {
                    DeviceId = instructionDraft.Device.Id,
                    AllowedActivities = instructionDraft.OnlyFor,
                    ForbiddenActivities = instructionDraft.ExceptFor,
                    Path = instructionPath,
                    InstructorId = instructor.Id,
                    InstructedId = user.Id,
                    CreatedAt = DateTime.Now,
                };

                instruction = await _instructionDataService.Create(instruction);

                instructions.Add(instruction);
            }

            ArchiveOlderInstructions(instructions);
        }

        private async void ArchiveOlderInstructions(IEnumerable<Instruction> instructions)
        {
            foreach (var newInstruction in instructions)
            {
                var oldInstructions = (await _instructionDataService.Where(i =>
                    i.DeviceId == newInstruction.DeviceId && i.InstructedId == newInstruction.InstructedId &&
                    i.CreatedAt < newInstruction.CreatedAt && i.Id != newInstruction.Id)).ToArray();

                _instructionPdfService.ArchiveOlderInstructions(
                    oldInstructions.Where(o => o.Path != newInstruction.Path));

                foreach (var oldInstruction in oldInstructions)
                {
                    oldInstruction.IsArchived = true;
                    await _instructionDataService.Update(oldInstruction.Id, oldInstruction);
                }
            }
        }

        private async Task<User> GetUser(UserDraft userDraft)
        {
            var existingUser = await _userDataService.FirstOrDefault(u => u.Email == userDraft.Email);

            if (!(existingUser is null)) return existingUser;

            var user = new User
            {
                FirstName = userDraft.FirstName,
                LastName = userDraft.LastName,
                Email = userDraft.Email,
            };

            return await _userDataService.Create(user);
        }
    }
}
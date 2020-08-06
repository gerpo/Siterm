using System.IO;
using System.Linq;
using Siterm.Domain.Models;
using Siterm.EntityFramework.Services;
using Siterm.Support.Misc;

namespace Siterm.DatabaseInitialization.Services
{
    internal class UserImporter : IImporter
    {
        private readonly InstructionDataService _instructionDataService;
        private readonly UserDataService _userDataService;

        public UserImporter(InstructionDataService instructionDataService,
            UserDataService userDataService)
        {
            _instructionDataService = instructionDataService;
            _userDataService = userDataService;
        }

        public int Order => 5;

        public async void Execute()
        {
            var instructions = await _instructionDataService.Where(i => string.IsNullOrEmpty(i.OldInstructedString));
            var users = (await _userDataService.GetAll()).ToList();

            foreach (var instruction in instructions)
            {
                var instructionInfo = new FileInfo(instruction.Path);
                var instructedName = instructionInfo.Name.Split("_")[0];
                if (!Helper.IsValidEmail(instructedName)) continue;

                var user = users.FirstOrDefault(u => u.Email == instructedName);

                if (user is null)
                {
                    user = new User {Email = instructedName};
                    users.Add(user);

                    var mailSplit = instructedName.Split("@")[0].Split(".");

                    if (mailSplit.Length != 2) continue;
                    user.FirstName = mailSplit[0];
                    user.LastName = mailSplit[1];
                }

                instruction.Instructed = user;

                await _instructionDataService.Update(instruction.Id, instruction);
            }
        }
    }
}
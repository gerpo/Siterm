using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Siterm.Domain.Models;
using Siterm.EntityFramework.Services;
using Siterm.Settings.Models;
using Siterm.Settings.Services;
using Siterm.Support.Misc;

namespace Siterm.DatabaseInitialization.Services
{
    public class InstructionImporter : IImporter
    {
        private const string Extension = "pdf";
        private readonly DeviceDataService _deviceDataService;
        private readonly InstructionDataService _instructionDataService;
        private readonly string _instructionFolderName;
        private readonly IEnumerable<string> _nameFilterList = new string[] { };

        public InstructionImporter(SettingsProvider settingsProvider, DeviceDataService deviceDataService,
            InstructionDataService instructionDataService)
        {
            _deviceDataService = deviceDataService;
            _instructionDataService = instructionDataService;

            _instructionFolderName = settingsProvider.GetSetting(SettingName.InstructionFolderName).Value;
        }

        public int Order => 4;

        public async void Execute()
        {
            if (string.IsNullOrEmpty(_instructionFolderName)) return;

            var allDevices = await _deviceDataService.GetAll();
            var oldInstructions = await _instructionDataService.GetAll();

            foreach (var device in allDevices)
            {
                var instructionPath = Path.Join(device.Path, _instructionFolderName);
                var dirInfo = new DirectoryInfo(instructionPath);

                if (!dirInfo.Exists) continue;

                var instructions = dirInfo.GetFiles($"*.{Extension}")
                    .Select(instructionInfo => new
                        {instructionInfo, nameSplit = instructionInfo.Name.Replace($".{Extension}", "").Split('_')})
                    .Where(i => i.nameSplit.Length == 2 &&
                                DateTime.TryParse(i.nameSplit[1],
                                    out _)) // check if the folder name has format of USERNAME_DATE
                    .Where(i => !_nameFilterList.Contains(i.instructionInfo
                        .Name)) // check if folder name is in filter list
                    .Select(i =>
                    {
                        var newInstruction = new Instruction
                        {
                            Path = i.instructionInfo.FullName,
                            CreatedAt = DateTime.Parse(i.nameSplit[1]),
                            Device = device
                        };
                        if (!Helper.IsValidEmail(i.nameSplit[0])) newInstruction.OldInstructedString = i.nameSplit[0];

                        return newInstruction;
                    })
                    .ToList();

                var filteredList = instructions.Where(s => oldInstructions.All(os => os.Path != s.Path)).ToList();

                device.Instructions = filteredList;

                await _deviceDataService.Update(device.Id, device);
            }
        }
    }
}
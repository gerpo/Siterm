using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Siterm.Domain.Models;
using Siterm.EntityFramework.Services;
using Siterm.Settings.Models;
using Siterm.Settings.Services;
using System.Threading.Tasks;

namespace Siterm.DatabaseInitialization.Services
{
    public class SubstanceImporter : IImporter
    {
        private readonly SubstanceDataService _dataService;

        private readonly IEnumerable<string> _nameFilterList = new[] {"ZZ_Arbeitsordner"};
        private readonly string _substancePath;

        public SubstanceImporter(SettingsProvider settingsProvider, SubstanceDataService dataService)
        {
            _dataService = dataService;
            _substancePath = settingsProvider.GetSetting(SettingName.SubstancePath).Value;
        }

        public int Order => 1;

        public async Task Execute()
        {
            var pathInfo = new DirectoryInfo(_substancePath);
            if (!pathInfo.Exists) return;

            var oldSubstance = await _dataService.GetAll();

            var substanceList = pathInfo.GetDirectories().Where(d => !_nameFilterList.Contains(d.Name))
                .Select(directoryInfo =>
                    new Substance {Path = directoryInfo.FullName, Name = directoryInfo.Name}).ToList();

            var filteredList = substanceList.Where(s => oldSubstance.All(os => os.Path != s.Path)).ToList();

            await _dataService.CreateAll(filteredList);

            Report(substanceList.Count, filteredList.Count());
        }

        private void Report(int nFound, int nAdded)
        {
            Console.WriteLine($"In Path {_substancePath}:");
            Console.WriteLine($"{nFound} Substances found and {nAdded} added");
        }
    }
}
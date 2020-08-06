using System.Collections.Generic;
using System.Linq;
using Siterm.EntityFramework.Services;
using Siterm.Settings.Models;
using Siterm.Settings.Services;
using Siterm.Substance.Models;
using Siterm.Support.Misc;

namespace Siterm.WPF.ViewModels
{
    public class SubstanceViewModel : BaseViewModel, ITabItemViewModel
    {
        private readonly SubstanceDataService _substanceDataService;

        public SubstanceViewModel(SettingsProvider settingProvider, SubstanceDataService substanceDataService)
        {
            _substanceDataService = substanceDataService;
            var substancePath = settingProvider.GetSetting(SettingName.SubstancePath).Value;
            SetSubstances(substancePath);
        }

        public IList<SubstanceTreeViewItem> Substances { get; private set; }

        public string Header => UiStrings.SubstanceTabHeader;
        public int Position => 3;

        private async void SetSubstances(string substancePath)
        {
            //if (string.IsNullOrEmpty(substancePath) || !Directory.Exists(substancePath)) return;

            //var substances = new DirectoryInfo(substancePath);

            //var substanceTreeViewItemList = substances.GetDirectories().Select(directoryInfo => new SubstanceTreeViewItem(new Domain.Models.Substance {Name = directoryInfo.Name, Path = directoryInfo.FullName})).ToList();

            //Substances = substanceTreeViewItemList;

            Substances = (await _substanceDataService.GetAll()).OrderBy(s => s.Name).Select(s => new SubstanceTreeViewItem(s)).ToList();
        }
    }
}
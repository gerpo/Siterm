using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Siterm.Settings.Models;
using Siterm.Settings.Services;
using Siterm.Substance.Models;
using Siterm.Support.Misc;

namespace Siterm.WPF.ViewModels
{
    public class SubstanceViewModel : BaseViewModel, ITabItemViewModel
    {
        public SubstanceViewModel(SettingsProvider settingProvider)
        {
            var substancePath = settingProvider.GetSetting(SettingName.SubstancePath).Value;
            SetSubstances(substancePath);
        }

        private void SetSubstances(string substancePath)
        {
            if (string.IsNullOrEmpty(substancePath) || !Directory.Exists(substancePath)) return;

            var substances = new DirectoryInfo(substancePath);

            var substanceTreeViewItemList = substances.GetDirectories().Select(directoryInfo => new SubstanceTreeViewItem(new Domain.Models.Substance {Name = directoryInfo.Name, Path = directoryInfo.FullName})).ToList();

            Substances = substanceTreeViewItemList;
        }

        public string Header => UiStrings.SubstanceTabHeader;
        public int Position => 3;

        public IList<SubstanceTreeViewItem> Substances { get; private set; }
    }
}
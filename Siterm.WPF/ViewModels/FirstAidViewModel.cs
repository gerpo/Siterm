using System.Collections.ObjectModel;
using System.Windows.Documents;
using Siterm.Domain.Models;
using Siterm.Excel.Services;
using Siterm.Settings.Models;
using Siterm.Settings.Services;
using Siterm.Support.Misc;
using Siterm.Support.Services;

namespace Siterm.WPF.ViewModels
{
    public class FirstAidViewModel : BaseViewModel, ITabItemViewModel
    {
        public FirstAidViewModel(SettingsProvider settingsProvider, RtfToFlowConverter rtfToFlowConverter)
        {
            FirstAidInfoFile =
                rtfToFlowConverter.CreateFlowDocument(settingsProvider.GetSetting(SettingName.FirstResponderInfoFile)
                    .Value);

            FirstResponders =
                ReadFirstAidExcelService.ReadFile(
                    settingsProvider.GetSetting(SettingName.FirstResponderExcelFile).Value);
        }

        public ObservableCollection<FirstResponder> FirstResponders { get; set; }

        public FlowDocument FirstAidInfoFile { get; set; }

        public string Header => UiStrings.FirstAidTabHeader;
        public int Position => 2;
    }
}
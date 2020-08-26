using System.Windows.Documents;
using Siterm.Settings.Models;
using Siterm.Settings.Services;
using Siterm.Support.Misc;
using Siterm.Support.Services;

namespace Siterm.WPF.ViewModels
{
    public class GeneralViewModel : BaseViewModel, ITabItemViewModel
    {

        public FlowDocument GeneralTemplateFile { get; }

        public string Header => UiStrings.GeneralTabHeader;
        public int Position => 1;

        public GeneralViewModel(SettingsProvider settingsProvider, RtfToFlowConverter rtfToFlowConverter)
        {
            var infoFilePath = settingsProvider.GetSetting(SettingName.HomeInfoFile).Value;
            GeneralTemplateFile = rtfToFlowConverter.CreateFlowDocument(infoFilePath);
        }
    }
}
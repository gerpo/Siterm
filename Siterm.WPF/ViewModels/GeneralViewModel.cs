using System.Windows.Documents;
using Siterm.Support.Misc;
using Siterm.Support.Services;

namespace Siterm.WPF.ViewModels
{
    public class GeneralViewModel : BaseViewModel, ITabItemViewModel
    {

        public FlowDocument GeneralTemplateFile { get; }

        public string Header => UiStrings.GeneralTabHeader;
        public int Position => 1;

        public GeneralViewModel(RtfToFlowConverter rtfToFlowConverter)
        {
            GeneralTemplateFile = rtfToFlowConverter.CreateFlowDocument(
                @"C:\Users\Adam\Documents\Sicherheit\09_Organisation\GUI_Inhalte\home.rtf");
        }
    }
}
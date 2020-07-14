namespace Siterm.WPF.ViewModels
{
    public class DeviceViewModel : BaseViewModel, ITabItemViewModel
    {
        public string Header => UiStrings.DeviceTabHeader;
        public int Position => 3;
    }
}
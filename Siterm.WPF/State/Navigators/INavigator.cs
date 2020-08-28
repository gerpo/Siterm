using Siterm.Support.Misc;

namespace Siterm.WPF.State.Navigators
{
    public enum ViewType
    {
        General,
        FirstAid,
        Device
    }

    public interface INavigator
    {
        BaseViewModel CurrentViewModel { get; set; }
        //ICommand UpdateCurrentViewModelCommand { get; }
    }
}
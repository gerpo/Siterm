using Siterm.Support.Misc;

namespace Siterm.WPF.State.Navigators
{
    public class Navigator : INavigator
    {
        public BaseViewModel CurrentViewModel { get; set; }
        //public ICommand UpdateCurrentViewModelCommand => new UpdateCurrentViewModelCommand(this);
    }
}
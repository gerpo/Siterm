using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Siterm.WPF.ViewModels;

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

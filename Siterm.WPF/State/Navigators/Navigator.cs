using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Siterm.Support.Misc;
using Siterm.WPF.ViewModels;

namespace Siterm.WPF.State.Navigators
{
    public class Navigator: INavigator
    {
        public BaseViewModel CurrentViewModel { get; set; }
        //public ICommand UpdateCurrentViewModelCommand => new UpdateCurrentViewModelCommand(this);
    }
}

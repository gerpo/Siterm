using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Siterm.Support.Misc
{
    public class RelayCommand: ICommand
    {
        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            _executeMethod(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        private readonly Action<object> _executeMethod;

        public RelayCommand(Action<object> executeMethod)
        {
            _executeMethod = executeMethod;
        }
    }
}

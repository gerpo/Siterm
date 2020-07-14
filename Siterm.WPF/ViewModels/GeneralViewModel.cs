using System;
using System.Collections.Generic;
using System.Text;

namespace Siterm.WPF.ViewModels
{
    public class GeneralViewModel : BaseViewModel, ITabItemViewModel
    {
        public string Header => UiStrings.GeneralTabHeader;
        public int Position => 1;
    }
}

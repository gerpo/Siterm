using System;
using System.Collections.Generic;
using System.Text;

namespace Siterm.WPF.ViewModels
{
    public class FirstAidViewModel :BaseViewModel, ITabItemViewModel
    {
        public string Header => UiStrings.FirstAidTabHeader;
        public int Position => 2;
    }
}

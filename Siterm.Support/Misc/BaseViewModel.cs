using System;

namespace Siterm.Support.Misc
{
    public class BaseViewModel : CanNotifyPropertyChanged
    {
        public event EventHandler ClosingRequest;

        protected void SentClosingRequest()
        {
            ClosingRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}
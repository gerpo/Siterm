using System.Collections.Generic;
using Siterm.Support.Misc;

namespace Siterm.Support.ControlModels
{
    public class GenericTreeViewItem<T>: CanNotifyPropertyChanged
    {
        public T Model { get; set; }
        public IList<FileSystemObject> Children { get; set; }
    }
}
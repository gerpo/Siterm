using System;
using System.Collections.Generic;
using System.Text;

namespace Siterm.Support.ControlModels
{
    public class GenericTreeViewItem<T>
    {
        public T Model { get; set; }
        public IList<FileSystemObject> Children { get; set; }
    }
}

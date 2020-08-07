using Siterm.Support.Misc;

namespace Siterm.Support.ControlModels
{
    public class FileSystemObject : CanNotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Path { get; protected set; }
    }
}
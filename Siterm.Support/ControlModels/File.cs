using System.IO;

namespace Siterm.Support.ControlModels
{
    public class File : FileSystemObject
    {
        public File(FileSystemInfo pathInfo)
        {
            Name = pathInfo.Name;
            Path = pathInfo.FullName;
        }
    }
}
using System.Diagnostics;
using System.IO;
using Siterm.Support.Misc;

namespace Siterm.Support.ControlModels
{
    public class File : FileSystemObject
    {
        public File(FileSystemInfo pathInfo)
        {
            Name = pathInfo.Name;
            Path = pathInfo.FullName;
        }

        public void Open()
        {
            Helper.OpenFile(Path);
        }
    }
}
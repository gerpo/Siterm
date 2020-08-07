using System.Diagnostics;
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

        public void Open()
        {
            if (string.IsNullOrEmpty(Path) || !System.IO.File.Exists(Path)) return;
            Process.Start(new ProcessStartInfo(Path) { UseShellExecute = true });
        }
    }
}
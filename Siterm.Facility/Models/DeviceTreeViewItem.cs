using System.Collections.Generic;
using System.IO;
using System.Linq;
using Siterm.Domain.Models;
using Siterm.Support.ControlModels;
using File = Siterm.Support.ControlModels.File;

namespace Siterm.Facility.Models
{
    public class DeviceTreeViewItem : GenericTreeViewItem<Device>
    {
        public DeviceTreeViewItem(Device device)
        {
            Model = device;
            Children = GenerateFileSystemObjects();
        }

        private IList<FileSystemObject> GenerateFileSystemObjects()
        {
            var devicePath = Model.Path;
            var devicePathInfo = new DirectoryInfo(devicePath);

            if (!devicePathInfo.Exists) return null;

            var childrenList = devicePathInfo.GetDirectories().Select(directoryInfo => new Folder(directoryInfo))
                .Cast<FileSystemObject>().ToList();
            childrenList.AddRange(devicePathInfo.GetFiles().Select(fileInfo => new File(fileInfo)));

            return childrenList;
        }
    }
}
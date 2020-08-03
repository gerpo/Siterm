using System.Collections.Generic;
using System.IO;
using System.Linq;
using Siterm.Support.ControlModels;
using File = Siterm.Support.ControlModels.File;

namespace Siterm.Substance.Models
{
    public class SubstanceTreeViewItem : GenericTreeViewItem<Domain.Models.Substance>
    {
        public SubstanceTreeViewItem(Domain.Models.Substance substance)
        {
            Model = substance;
            Children = GenerateFileSystemObjects();
        }

        private IList<FileSystemObject> GenerateFileSystemObjects()
        {
            var substancePath = Model.Path;
            var substancePathInfo = new DirectoryInfo(substancePath);

            if (!substancePathInfo.Exists) return null;

            var childrenList = substancePathInfo.GetDirectories().Select(directoryInfo => new Folder(directoryInfo)).Cast<FileSystemObject>().ToList();
            childrenList.AddRange(substancePathInfo.GetFiles().Select(fileInfo => new File(fileInfo)).Cast<FileSystemObject>());

            return childrenList;
        }
    }
}
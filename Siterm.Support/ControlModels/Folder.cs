﻿using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Siterm.Support.ControlModels
{
    public class Folder : FileSystemObject
    {
        private readonly DirectoryInfo _pathInfo;
        private IList<FileSystemObject> _children;

        public Folder(DirectoryInfo pathInfo)
        {
            _pathInfo = pathInfo;
            Name = pathInfo.Name;
            Path = pathInfo.FullName;
        }

        public IList<FileSystemObject> Children => _children ??= GetChildren();

        private IList<FileSystemObject> GetChildren()
        {
            var childrenList = _pathInfo.GetDirectories().Select(directoryInfo => new Folder(directoryInfo)).Cast<FileSystemObject>().ToList();
            childrenList.AddRange(_pathInfo.GetFiles().Select(fileInfo => new File(fileInfo)).Cast<FileSystemObject>());

            return childrenList;
        }
    }
}
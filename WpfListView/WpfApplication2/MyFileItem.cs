using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication2
{
    public class MyFileInfo
    {
        public long Size { get; private set; }
        public string Name { get; private set; }
        public IList<MyFileInfo> SubFiles { get; set; }
        public DateTime LastEdit { get; private set; }
        public int FileCount { get; private set; }
        public FileType Type { get; private set; }
        public MyFileInfo Parent { get; private set; }
        public string FullPath { get; private set; }

        public MyFileInfo(MyFileInfo parent)
        {
            Parent = parent;
        }

        public MyFileInfo(string path, MyFileInfo parent)
        {
            FileCount = 0;
            FullPath = path;
            Name = path.Substring(path.LastIndexOf("\\") + 1);
            SubFiles = new List<MyFileInfo>();
            FileAttributes attr = File.GetAttributes(path);
            LastEdit = Directory.GetLastWriteTime(path);
            Parent = parent;

            if (attr.HasFlag(FileAttributes.Directory))
            {
                Type = FileType.Directory;
                foreach (string dir in Directory.GetDirectories(path))
                {
                    MyFileInfo subDir = new MyFileInfo(dir, this);
                    SubFiles.Add(subDir);
                    FileCount += subDir.FileCount;
                }

                foreach (string file in Directory.GetFiles(path))
                {
                    MyFileInfo subDir = new MyFileInfo(file, this);
                    SubFiles.Add(subDir);
                    FileCount += 1;
                }

                foreach (MyFileInfo f in SubFiles)
                {
                    Size += f.Size;
                }
            }
            else
            {
                Type = FileType.File;
                Size = new FileInfo(path).Length;
                FileCount = 1;

            }
            Name = path.Substring(path.LastIndexOf("\\") + 1);
            if (Name.Trim().Equals(""))
            {
                Name = path;
                Type = FileType.RootDrive;
            }

        }


    }
        public enum FileType
        {
            Directory,
            File,
            RootDrive
        };
}

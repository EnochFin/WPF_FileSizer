using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class MyFileInfo
    {


        public FileType Type { get; private set; }

        public string Name { get; private set; }

        public long Size { get; private set; }

        public int FileCount { get; private set; }

        public MyFileInfo Parent { get; private set; }

        public IList<MyFileInfo> SubFiles { get; set; }

        public DateTime LastEdit { get; private set; }

        private MyFileInfo(string path, MyFileInfo parent)
        {
            FileCount = 0;
            Name = path.Substring(path.LastIndexOf("\\") + 1);
            Parent = parent;
            SubFiles = new List<MyFileInfo>();
            FileAttributes attr = File.GetAttributes(path);
            LastEdit = Directory.GetLastWriteTime(path);

            if (attr.HasFlag(FileAttributes.Directory))
            {
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
            }
            catch (UnauthorizedAccessException)
            {
               Type = FileType.File;
               Size = new FileInfo(path).Length;
                FileCount = 1;

            Name = path.Substring(path.LastIndexOf("\\") + 1);
            if (Name.Trim().Equals(""))
            {
                Name = path;
                Type = FileType.RootDrive;
            }

        }

        public MyFileInfo(String path)
        {
            Name = "__Parent";
            SubFiles = new List<MyFileInfo>();
            SubFiles.Add(new MyFileInfo(path, this));
        }

    }

    public enum FileType
    {
        Directory,
        File
    };

}
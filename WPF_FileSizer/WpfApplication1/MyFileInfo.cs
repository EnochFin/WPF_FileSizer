using System;
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
        public string Name { get; private set; }
        public double Size { get; private set; }
        public MyFileInfo Parent { get; private set; }

        public IList<MyFileInfo> SubFiles { get; set; }

        private MyFileInfo(string path, MyFileInfo parent)
        {
            Name = path.Substring(path.LastIndexOf("\\") + 1);
            Parent = parent;
            SubFiles = new List<MyFileInfo>();
            FileAttributes attr = File.GetAttributes(path);

            if (attr.HasFlag(FileAttributes.Directory))
            {
                foreach (string dir in Directory.GetDirectories(path))
                {
                    SubFiles.Add(new MyFileInfo(dir, this));
                }

                foreach (string file in Directory.GetFiles(path))
                {
                    SubFiles.Add(new MyFileInfo(file, this));
                }

                foreach (MyFileInfo f in SubFiles)
                {
                    Size += f.Size;
                }
            }
            else
            {
               Size = new FileInfo(path).Length;
                Size /= 1024;
            }



        }

        public MyFileInfo(String path)
        {
            Name = "__Parent";
            SubFiles = new List<MyFileInfo>();
            SubFiles.Add(new MyFileInfo(path, this));
        }
    }
}
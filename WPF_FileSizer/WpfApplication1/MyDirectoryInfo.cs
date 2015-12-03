using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class MyDirectoryInfo
    {
        public string Name { get; private set; }
        public long Size { get; private set; }
        public MyDirectoryInfo Parent { get; private set; }

        public IList<MyDirectoryInfo> Subdirectories { get; set; }

        private MyDirectoryInfo(string path, MyDirectoryInfo parent)
        {
            Name = path.Substring(path.LastIndexOf("\\") + 1);
            Parent = parent;
            Subdirectories = new List<MyDirectoryInfo>();
            Size = 5;
            foreach (String dir in Directory.GetDirectories(path))
            {
                Subdirectories.Add(new MyDirectoryInfo(dir, this));
            }

            //foreach(string s in Directory.GetFiles(path))
            //{
            //    FileInfo f = new FileInfo(s);
            //    Size += f.Length;
            //}

            foreach (MyDirectoryInfo d in Subdirectories)
            {
                Size += d.Size;
            }
        }

        public MyDirectoryInfo(String path)
        {
            Name = "__Parent";
            Subdirectories = new List<MyDirectoryInfo>();
            Subdirectories.Add(new MyDirectoryInfo(path, this));

            //Size += size of self
        }
    }
}

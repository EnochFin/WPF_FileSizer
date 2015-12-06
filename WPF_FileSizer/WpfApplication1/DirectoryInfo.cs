using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class DirectoryInfo
    {
        public string Name { get; private set; }
        public long Size { get; private set; }
        public DirectoryInfo Parent { get; private set; }

        public IList<DirectoryInfo> Subdirectories { get; set; }

        private DirectoryInfo(string path, DirectoryInfo parent)
        {
            Name = path.Substring(path.LastIndexOf("\\") + 1);
            Parent = parent;
            Subdirectories = new List<DirectoryInfo>();
            Size = 5;
            foreach (String dir in Directory.GetDirectories(path))
            {
                Subdirectories.Add(new DirectoryInfo(dir, this));
            }

            foreach (DirectoryInfo d in Subdirectories)
            {
                Size += d.Size;
            }


        }

        public DirectoryInfo(String path)
        {
            Name = "__Parent";
            Subdirectories = new List<DirectoryInfo>();
            Subdirectories.Add(new DirectoryInfo(path, this));
        }
    }
}

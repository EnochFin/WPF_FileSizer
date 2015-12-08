using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication2
{
    class MyDirInfo
    {

        public string Name { get; private set; }

        public string FullPath;

        public MyDirInfo Parent { get; private set; }

        public IList<MyDirInfo> SubFiles { get; set; }
        /// <summary>
        /// Enoch's contribution, I decided my feature would be to include a sidebar that would start at a root directory ex: C:\ and would show only directories
        /// if you click on any directory in the tree, it will rediretc you to that directory in the file explorer carver and I made.
        /// 
        /// If I would be able to work more on this, I would delay the population of the Subdirectories until after you selected a directory in the side panel so it would be faster on startup
        /// and the side panel would probably be a popout that would would show only when you wanted it to, while working on this project I learned about Hierarchical Data Templates and 
        /// discovered a use for private constructors, like in the class below.
        /// 
        /// Through this process I definitely feel more comfortable in binding, Data Templates, Styles, file exploration, and styling in general.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        private MyDirInfo(string path, MyDirInfo parent)
        {
            Name = path.Substring(path.LastIndexOf("\\") + 1);
            Parent = parent;
            SubFiles = new List<MyDirInfo>();
            FileAttributes attr = File.GetAttributes(path);

            if (attr.HasFlag(FileAttributes.Directory))
            {
                try
                {
                    FullPath = path;
                    foreach (string dir in Directory.GetDirectories(path))
                    {
                        SubFiles.Add(new MyDirInfo(dir, this));
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    Name += (" - UnAuthorized");
                }
            }

        }

        public MyDirInfo(String path)
        {
            Name = "__Parent";
            SubFiles = new List<MyDirInfo>();
            SubFiles.Add(new MyDirInfo(path, this));
        }

    }

}
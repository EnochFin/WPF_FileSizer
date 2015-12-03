using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class MyFileInfo
    {
        public string Name { get; private set; }
        public int Size { get; private set; }
        public MyDirectoryInfo Parent { get; private set; }

        public MyFileInfo(string path)
        {

        }
    }
}

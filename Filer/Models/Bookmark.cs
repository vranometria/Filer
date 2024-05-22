using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filer.Models
{
    public class Bookmark
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public Bookmark(string path)
        {
            Name = path;
            Path = path;
        }
    }
}

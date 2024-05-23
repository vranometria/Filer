using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filer.Models
{
    public class Bookmark
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Path { get; set; }

        public bool IsFolder => Directory.Exists(Path);

        public Bookmark(string path)
        {
            Name = path;
            Path = path;
        }
    }
}

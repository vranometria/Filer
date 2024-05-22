using Filer.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filer
{
    public static class Utils
    {
        public static List<string> GetObjects(string directoryPath)
        {             
            var objects = new List<string>();
            foreach (var file in Directory.GetFiles(directoryPath)) { objects.Add(file); }
            foreach (var dir in Directory.GetDirectories(directoryPath)) { objects.Add(dir); }
            return objects;
        }
    }
}

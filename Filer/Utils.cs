using Filer.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows;

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

        public static void Execute(string filePath)
        {
            try
            {
                ProcessStartInfo processStartInfo = new(filePath) { UseShellExecute = true };
                Process.Start(processStartInfo);
            }
            catch (Win32Exception win32e) { 
                MessageBox.Show(win32e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

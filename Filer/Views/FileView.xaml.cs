using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;
using Filer.Models;

namespace Filer.Views
{
    /// <summary>
    /// FileView.xaml の相互作用ロジック
    /// </summary>
    public partial class FileView : UserControl
    {
        public string Path { get; private set; } = "";

        public ObjectType ObjectType { get; private set; }


        public FileView()
        {
            InitializeComponent();
        }

        public FileView(string path):this()
        {
            Path = path;
            ObjectType = File.GetAttributes(path).HasFlag(FileAttributes.Directory) ? ObjectType.Directory : ObjectType.File;
        }

        private void FileNameLabel_Loaded(object sender, RoutedEventArgs e)
        {
            Label label = (Label)sender;
            label.Content = System.IO.Path.GetFileName(Path);
        }
    }
}

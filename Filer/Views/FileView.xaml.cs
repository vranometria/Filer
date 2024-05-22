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

namespace Filer.Views
{
    /// <summary>
    /// FileView.xaml の相互作用ロジック
    /// </summary>
    public partial class FileView : UserControl
    {
        private string FilePath { get; set; } = "";


        public FileView()
        {
            InitializeComponent();
        }

        public FileView(string filePath):this()
        {
            FilePath = filePath;
        }

        private void FileNameLabel_Loaded(object sender, RoutedEventArgs e)
        {
            Label label = (Label)sender;
            label.Content = Path.GetFileName(FilePath);
        }
    }
}

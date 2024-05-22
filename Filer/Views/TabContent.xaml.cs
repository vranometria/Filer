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
using System.Security.Policy;

namespace Filer.Views
{
    /// <summary>
    /// TabContent.xaml の相互作用ロジック
    /// </summary>
    public partial class TabContent : UserControl
    {
        public TabContent()
        {
            InitializeComponent();
        }

        private void ShowFileList(string directryPath)
        {
            if (string.IsNullOrEmpty(directryPath)){ return; }

            FileViewList.Items.Clear();
            if (Directory.Exists(directryPath)){ Utils.GetObjects(directryPath).ForEach(o => FileViewList.Items.Add(new FileView(o)));}
        }

        private void UrlTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                var url = UrlTextBox.Text;
                ShowFileList(url);
            }
        }

        private void FileViewList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var fileView = (FileView)FileViewList.SelectedItem;
            if (fileView.ObjectType == ObjectType.Directory){ 
                UrlTextBox.Text = fileView.Path;
                ShowFileList(fileView.Path); 
            }
        }
    }
}

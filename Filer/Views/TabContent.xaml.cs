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
    /// TabContent.xaml の相互作用ロジック
    /// </summary>
    public partial class TabContent : UserControl
    {
        public TabContent()
        {
            InitializeComponent();
        }

        private void UrlTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                var url = UrlTextBox.Text;
                if (string.IsNullOrEmpty(url))
                {
                    return;
                }

                FileViewList.Items.Clear();

                //ディレクトリが存在する場合は、中身のファイルをStackAreaに表示
                if (Directory.Exists(url))
                {
                    foreach (var file in Directory.GetFiles(url))
                    {
                        FileViewList.Items.Add(new FileView(file));
                    }

                    foreach (var dir in Directory.GetDirectories(url))
                    {
                        FileViewList.Items.Add(new FileView(dir));
                    }
                }
            }
        }
    }
}

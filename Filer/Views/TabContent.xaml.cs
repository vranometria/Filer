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
        private AppDataManager AppDataManager { get; set; } = AppDataManager.GetInstance;


        public TabContent()
        {
            InitializeComponent();
        }

        public TabContent(string path): this()
        {
            UrlTextBox.Text = path;
            ShowFileList(path);
        }

        private void ShowFileList(string directryPath)
        {
            if (string.IsNullOrEmpty(directryPath)){ return; }

            FileViewList.Items.Clear();
            if (Directory.Exists(directryPath)){ Utils.GetObjects(directryPath).ForEach(o => FileViewList.Items.Add(new FileView(o)));}
        }

        private void Reload() 
        {
            ShowFileList(UrlTextBox.Text);
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
            try
            {
                var fileView = (FileView)FileViewList.SelectedItem;
                if (fileView.ObjectType == ObjectType.Directory)
                {
                    UrlTextBox.Text = fileView.Path;
                    ShowFileList(fileView.Path);
                }
                else
                {
                    Utils.Execute(fileView.Path);
                }
            }
            catch (UnauthorizedAccessException unauthorized)
            {
                MessageBox.Show(unauthorized.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BookmarkContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var fileView = (FileView)FileViewList.SelectedItem;
            if (fileView == null) { return; }

            AppDataManager.AddBookmark(new Bookmark(fileView.Path));
        }

        private void UpLayerButton_Click(object sender, RoutedEventArgs e)
        {
            string currentPath = UrlTextBox.Text;
            DirectoryInfo? parentDir = Directory.GetParent(currentPath);
            if ( parentDir == null ){return;}

            UrlTextBox.Text = parentDir.FullName;
            ShowFileList(parentDir.FullName);
        }

        private void UserControl_Drop(object sender, DragEventArgs e)
        {
            //現在ディレクトリが存在しない場合は何もしない
            if (!Directory.Exists(UrlTextBox.Text)) { return; }
            string currentDir = UrlTextBox.Text;

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                List<string> paths = ((string[])e.Data.GetData(DataFormats.FileDrop)).ToList();

                paths.ForEach(path =>
                {
                    //ファイルを現在フォルダに移動する
                    string fileName = Path.GetFileName(path);
                    string destPath = Path.Combine(currentDir, fileName);
                    //すでに同名のファイルが存在する場合は確認メッセージを表示し、OKの場合は削除した後に移動する
                    if (File.Exists(destPath))
                    {
                        MessageBoxResult result = MessageBox.Show($"{fileName}はすでに存在します。上書きしますか？", "確認", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            Utils.DeleteObject(destPath);
                            Utils.MoveObject(path, destPath);
                            Reload();
                        }
                    }
                    else 
                    {
                        Utils.MoveObject(path, destPath);
                        Reload();
                    }
                });
            }
        }

        private void DeleteContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var fileView = (FileView)FileViewList.SelectedItem;
            if (fileView == null) { return; }

            if (!Utils.IsObjectExists(fileView.Path)) 
            {
                Reload();
                return;
            }

            MessageBoxResult result = MessageBox.Show($"{fileView.Name}を削除しますか？", "確認", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (fileView.ObjectType == ObjectType.Directory)
                {
                    Directory.Delete(fileView.Path, true);
                }
                else
                {
                    File.Delete(fileView.Path);
                }
                Reload();
            }
        }
    }
}

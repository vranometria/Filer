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
using System.Printing;

namespace Filer.Views
{
    /// <summary>
    /// TabContent.xaml の相互作用ロジック
    /// </summary>
    public partial class TabContent : UserControl
    {
        private AppDataManager AppDataManager { get; set; } = AppDataManager.GetInstance;

        private List<FileView> FileViews = new();

        public TabContent()
        {
            InitializeComponent();
        }

        public TabContent(string path): this()
        {
            UrlTextBox.Text = path;
            Reload();
        }

        private void ShowFileList()
        {
            FileViewList.Items.Clear();
            FileViews.ForEach(fileView => FileViewList.Items.Add(fileView));
        }

        private void Reload() 
        {
            ShowFileList();
        }

        private void MovePage(string url) 
        {
            UrlTextBox.Text = url;
            FileViews = Utils.GetObjects(url).Select(o => new FileView(o)).ToList();
            ShowFileList();
        }

        private void UrlTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                var url = UrlTextBox.Text;

                if (Directory.Exists(url))
                {
                    MovePage(url);
                }
            }
        }

        private void FileViewList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var fileView = (FileView)FileViewList.SelectedItem;
                if (fileView.ObjectType == ObjectType.Directory)
                {
                    MovePage(fileView.FilePath);
                }
                else
                {
                    Utils.Execute(fileView.FilePath);
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

            AppDataManager.AddBookmark(new Bookmark(fileView.FilePath));
        }

        private void UpLayerButton_Click(object sender, RoutedEventArgs e)
        {
            string currentPath = UrlTextBox.Text;
            DirectoryInfo? parentDir = Directory.GetParent(currentPath);
            if ( parentDir == null ){return;}

            UrlTextBox.Text = parentDir.FullName;
            MovePage(parentDir.FullName);
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

            if (!Utils.IsObjectExists(fileView.FilePath)) 
            {
                Reload();
                return;
            }

            MessageBoxResult result = MessageBox.Show($"{fileView.Name}を削除しますか？", "確認", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (fileView.ObjectType == ObjectType.Directory)
                {
                    Directory.Delete(fileView.FilePath, true);
                }
                else
                {
                    File.Delete(fileView.FilePath);
                }
                Reload();
            }
        }


        private void FileViewList_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter && FileViewList.SelectedItem != null)
            {
                FileView fileView = (FileView)FileViewList.SelectedItem;
                MovePage(fileView.FilePath);
                return;
            }

            // backspaceキーが押された場合はsearchTextBoxのテキストの最後尾を削除する
            if (e.Key == Key.Back && SearchTextBox.Text.Length > 0)
            {
                SearchTextBox.Text = SearchTextBox.Text.Substring(0, SearchTextBox.Text.Length - 1);
                return;
            }

            if (Utils.IsAlphabetKey(e.Key) && e.Key != Key.OemMinus && e.Key != Key.OemPeriod )
            {
                SearchTextBox.Text += e.Key.ToString().ToLower();
                return;
            }

            if(Utils.IsNumberKey(e.Key))
            {
                SearchTextBox.Text += Utils.GetNumberKeyString(e.Key);
                return;
            }

            if(e.Key == Key.OemMinus)
            {
                SearchTextBox.Text += "-";
                return;
            }

            if(e.Key == Key.OemPeriod)
            {
                SearchTextBox.Text += ".";
                return;
            }

            if(e.Key == Key.OemComma) { SearchTextBox.Text += ","; return; }

            if(Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) && e.Key == Key.Oem102)
            {
                SearchTextBox.Text += "_";
                return;
            }

        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchWord = SearchTextBox.Text;

            if (string.IsNullOrEmpty(searchWord))
            {
                Reload();
            }

            FileViewList.Items.Clear();
            FileViews.Where( fv => fv.FileName.ToLower().StartsWith(searchWord)).ToList().ForEach(fv => FileViewList.Items.Add(fv));
        }
    }
}

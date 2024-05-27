using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Filer.Models;
using Filer.Views;
using Microsoft.VisualBasic;

namespace Filer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AppDataManager AppDataManager { get; set; } = AppDataManager.GetInstance;

        private HotkeyHelper HotkeyHelper { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            HotkeyHelper = new HotkeyHelper(this);
        }

        private void OpenTabPage(string path, string tabName = "")
        {
            TabItem tabItem = new()
            {
                Content = new TabContent(path),
                Width = double.NaN,
                Header = tabName,
            };
            Tab.Items.Add(tabItem);
            Tab.SelectedItem = tabItem;
        }

        private void TabAddMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TabItem tabItem = new() { 
                Content = new TabContent(),
                Width = double.NaN,
            };
            Tab.Items.Add(tabItem);
            Tab.SelectedItem = tabItem;
        }

        private void BookmarkMenuItem_MouseEnter(object sender, MouseEventArgs e)
        {
            BookmarkMenuItem.Items.Clear();
            AppDataManager.GetBookmarks().ForEach(bookmark =>
            {
                // inputboxを表示してリネームする
                MenuItem renameMenuItem = new() { Header = "Rename", Background = Utils.Background, Foreground = Brushes.White};
                renameMenuItem.Click += (s, args) =>
                {
                    string name = Interaction.InputBox(bookmark.Name, "ブックマーク名を変更します");
                    if (string.IsNullOrEmpty(name)) { return; }
                    bookmark.Name = name;
                    AppDataManager.UpdateBookmark(bookmark);
                };

                MenuItem removeMenuItem = new() { Header = "Remove", Background = Utils.Background, Foreground = Brushes.White};
                removeMenuItem.Click += (s, args) => { AppDataManager.RemoveBookmark(bookmark); };

                MenuItem menuItem = new()
                {
                    Header = bookmark.Name,
                    Foreground = Brushes.White,
                    Background = Utils.Background,
                    ContextMenu = new ContextMenu(){ Items = {renameMenuItem, removeMenuItem} },
                };
                menuItem.Click += (sender, e) => {
                    if (bookmark.IsFolder)
                    {
                        OpenTabPage(bookmark.Path, bookmark.Name);
                    }
                    else
                    {
                        Utils.Execute(bookmark.Path);
                    }
                };
                BookmarkMenuItem.Items.Add(menuItem);
            });
        }

        private void HotkeySettingMenuItem_Click(object sender, RoutedEventArgs e)
        {
            HotkeySettingWindow window = new(HotkeyHelper);
            window.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            HotkeyHelper.UnregisterAll();
        }
    }
}
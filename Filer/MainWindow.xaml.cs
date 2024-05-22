using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Filer.Views;

namespace Filer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AppDataManager AppDataManager { get; set; } = AppDataManager.GetInstance;

        public MainWindow()
        {
            InitializeComponent();
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
                MenuItem menuItem = new()
                {
                    Header = bookmark.Name,
                    Foreground = Brushes.White,
                    Background = new SolidColorBrush(Color.FromRgb(67, 67, 68)),
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
    }
}
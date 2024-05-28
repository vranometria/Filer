using Filer.Models;
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
using System.Windows.Shapes;

namespace Filer.Views
{
    /// <summary>
    /// HotkeySettingWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class HotkeySettingWindow : Window
    {
        private HotkeyHelper? HotkeyHelper { get; set; }

        private AppDataManager AppDataManager { get; set; } = AppDataManager.GetInstance;

        public HotkeySettingWindow()
        {
            InitializeComponent();
            RegisterKeyTextBox.Tag = Key.None;
        }

        public HotkeySettingWindow(HotkeyHelper hotkeyHelper):this()
        {
            HotkeyHelper = hotkeyHelper;
        }

        private void RegisterKeyTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            RegisterKeyTextBox.Text = e.Key.ToString();
            RegisterKeyTextBox.Tag = e.Key;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            Key key = (Key)RegisterKeyTextBox.Tag;
            bool? ctrl = ControlCheckBox.IsChecked;
            bool? shift = ShiftCheckBox.IsChecked;
            bool? alt = AltCheckBox.IsChecked;

            if( key == Key.None &&  ctrl == false && shift == false && alt == false )
            {
                MessageBox.Show("Please input key and check modifier key.");
                return;
            }

            if( HotkeyHelper != null )
            {
                ModifierKeys modifierKeys = ModifierKeys.None;
                if( ctrl == true ) modifierKeys |= ModifierKeys.Control;
                if( shift == true ) modifierKeys |= ModifierKeys.Shift;
                if( alt == true ) modifierKeys |= ModifierKeys.Alt;

                bool isSuccess = HotkeyHelper.RegisterActivateEvent(modifierKeys, key);
                if( isSuccess )
                {
                    MessageBox.Show("Register hotkey successfully.");
                    var hotkey = new Hotkey() { Key = key, Control = ctrl == true, Alt = alt == true, Shift = shift == true, Activate = true  };
                    AppDataManager.SetHotkey(hotkey);
                    AppDataManager.Save();
                }
                else
                {
                    MessageBox.Show("Failed to register hotkey.");
                }
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

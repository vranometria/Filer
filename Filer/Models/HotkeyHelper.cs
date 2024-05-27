using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace Filer.Models
{
    public class HotkeyHelper: IDisposable
    {
        private IntPtr WindowHandle { get; set; }

        private const int WINDOW_MESSAGE = 0x0312;

        private const int MAX_HOTKEY_ID = 0xC000;

        private Dictionary<int, HotkeyItem> HotkeyItems { get; set; } = [];

        private int ActivateHotkeyId { get; set; } = 0x0000;

        [DllImport("user32.dll")]
        private static extern int RegisterHotKey(IntPtr hWnd, int id, int modKey, int vKey);

        [DllImport("user32.dll")]
        private static extern int UnregisterHotKey(IntPtr hWnd, int id);

        private EventHandler ActivateEventHandler { get; set; } = (sender, e) =>
        {
            var window = Application.Current.MainWindow;
            if (window.WindowState == WindowState.Minimized) { window.WindowState = WindowState.Normal; }
            window.Activate();
        };

        public HotkeyHelper(Window window)
        {
            var host = new WindowInteropHelper(window);
            WindowHandle = host.Handle;
            ComponentDispatcher.ThreadPreprocessMessage += ComponentDispatcher_ThreadPreprocessMessage;
        }

        private void ComponentDispatcher_ThreadPreprocessMessage(ref MSG msg, ref bool handled)
        {
            if (msg.message != WINDOW_MESSAGE) { return; }

            var id = msg.wParam.ToInt32();
            var hotkeyItem = HotkeyItems[id];
            hotkeyItem?.Handler?.Invoke(this, EventArgs.Empty);
        }

        public bool RegisterActivateEvent(ModifierKeys modifierKey, Key key)
        {
            while ( ActivateHotkeyId < MAX_HOTKEY_ID )
            {
                int ret = RegisterHotKey(WindowHandle, ActivateHotkeyId, (int)modifierKey, KeyInterop.VirtualKeyFromKey(key));
                if (ret != 0)
                {
                    HotkeyItems[ActivateHotkeyId] = new HotkeyItem(modifierKey, key, ActivateEventHandler);
                    return true;
                }

                ActivateHotkeyId++;
            }
            return false;
        }

        public bool Unregister(int id)
        {
            return UnregisterHotKey(WindowHandle, id) != 0;
        }

        public void UnregisterAll()
        {
            foreach (var id in HotkeyItems.Keys)
            {
                Unregister(id);
            }
        }

        public void Dispose()
        {
            UnregisterAll();
        }
    }
}

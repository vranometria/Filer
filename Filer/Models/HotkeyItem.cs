using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Filer.Models
{
    public class HotkeyItem(ModifierKeys modifierKeys, Key key, EventHandler handler)
    {
        public ModifierKeys ModifierKeys { get; set; } = modifierKeys;

        public Key Key { get; set; } = key;

        public EventHandler Handler { get; set; } = handler;
    }
}

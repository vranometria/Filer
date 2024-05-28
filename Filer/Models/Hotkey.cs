using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Filer.Models
{
    public class Hotkey
    {
        public Key Key { get; set; }

        public bool Control { get; set; }

        public bool Shift { get; set; }

        public bool Alt { get; set; }

        public bool Activate { get; set; }


        public ModifierKeys ModifierKeys
        {
            get
            {
                ModifierKeys modifierKeys = ModifierKeys.None;
                if (Control) modifierKeys |= ModifierKeys.Control;
                if (Shift) modifierKeys |= ModifierKeys.Shift;
                if (Alt) modifierKeys |= ModifierKeys.Alt;
                return modifierKeys;
            }
        }
    }
}

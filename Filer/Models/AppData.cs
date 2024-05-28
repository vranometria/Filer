using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filer.Models
{
    public class AppData
    {
        public List<Bookmark> Bookmarks { get; set; } = [];

        public Hotkey Hotkey { get; set; } = new Hotkey();
    }
}

using Filer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.IO;
using System.Windows.Input;

namespace Filer
{
    public class AppDataManager
    {
        private static AppDataManager Instance { get; set; } = new AppDataManager();

        public static AppDataManager GetInstance => (Instance == null) ? (Instance = new AppDataManager()) : Instance;

        public const string APP_DATA_FILE = "app.json";

        private AppData AppData { get; set; } = new AppData();

        public Hotkey Hotkey => AppData.Hotkey;

        private AppDataManager()
        {
            if (File.Exists(APP_DATA_FILE))
            {
                AppData = LoadData() ?? new AppData();
            }
            else
            {
                AppData = new AppData();
            }
        }

        private AppData? LoadData() 
        {
            string s = File.ReadAllText(APP_DATA_FILE);
            return JsonSerializer.Deserialize<AppData>(s);
        }


        public void Save()
        {
            string s = JsonSerializer.Serialize(AppData);
            File.WriteAllText(APP_DATA_FILE, s);
        }

        public void AddBookmark(Bookmark bookmark)
        {
            AppData.Bookmarks.Add(bookmark);
        }

        public List<Bookmark> GetBookmarks()
        {
            return AppData.Bookmarks;
        }

        public void UpdateBookmark(Bookmark bookmark)
        {
            var index = AppData.Bookmarks.FindIndex(b => b.Id == bookmark.Id);
            AppData.Bookmarks[index] = bookmark;
        }

        public void RemoveBookmark(Bookmark bookmark)
        {
            AppData.Bookmarks.Remove(bookmark);
        }

        public void SetHotkey(Hotkey hotkey)
        {
            AppData.Hotkey = hotkey;
        }

        public bool IsHotkeySet()
        {
            return AppData.Hotkey.Activate;
        }
    }
}

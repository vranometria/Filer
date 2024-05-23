using Filer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filer
{
    public class AppDataManager
    {
        private static AppDataManager Instance { get; set; } = new AppDataManager();

        public static AppDataManager GetInstance => (Instance == null) ? (Instance = new AppDataManager()) : Instance;

        private AppData AppData { get; set; } = new AppData();

        private AppDataManager(){}

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
    }
}

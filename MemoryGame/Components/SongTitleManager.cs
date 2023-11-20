using System.Data;

namespace MemoryGame
{
    /// <summary>
    /// 歌曲標題管理器，用於管理和控制遊戲中的歌曲標題按鈕。
    /// </summary>
    public class SongTitleManager
    {
        private readonly Random random;
        public List<SongTitle> List { get; set; }
        public string Song { get; set; }
        public bool CanPick = true;
        public IManagerListener? Managerlistener;
        public SongTitle? PickSong;

        /// <summary>
        /// SongTitleManager 類別的構造函數，用於初始化管理歌曲標題按鈕的物件。
        /// </summary>
        public SongTitleManager()
        {
            random = new Random();
            List = new List<SongTitle>();
            Song = "";
        }

        /// <summary>
        /// 隨機分配按鈕的對應歌曲，確保每個按鈕都獲得一個隨機的歌曲識別鍵。
        /// </summary>
        public void RandomlyAssignKeys()
        {
            List = this.List.OrderBy(card => random.Next()).ToList();   // 使用亂數重新排列 "List" 中的項目，以實現隨機分配。
            // 從歌曲資料表中選取非空的文件鍵並轉為列表，以便後續的操作。
            List<string> keys = MainForm.SongDataTable.AsEnumerable()
                                .Select(row => row.Field<string>("File"))
                                .OfType<string>().ToList();
            keys = keys.OrderBy(x => random.Next()).ToList();// 使用亂數重新排列 "keys" 清單中的識別鍵，以實現隨機分配。
            if (keys.Count < List.Count)// 檢查 "keys" 清單中的元素數是否小於 "List" 清單中的元素數。
            {
                // 如果 "keys" 元素數不足，則拋出異常，指出無法進行隨機分配。
                throw new InvalidOperationException("Not enough File keys available for random assignment.");
            }
            for (int i = 0; i < List.Count; i++)// 遍歷 "List" 清單中的按鈕項目，並分配對應的歌曲識別鍵。
            {
                string key = keys[i];// 從 "keys" 清單中取得一個識別鍵。
                List[i].File = key;// 將識別鍵分配給 "List" 清單中的按鈕項目的 "File" 屬性。
            }
        }


        /// <summary>
        /// 從管理器中隨機獲取一首歌曲。
        /// </summary>
        /// <returns>隨機選取的歌曲檔案名</returns>
        public string GetRandomSong()
        {
            string song = List.ElementAt(random.Next(List.Count)).File; // 從列表中隨機選擇一首歌曲
            return song; // 返回選擇的歌曲
        }

        /// <summary>
        /// 檢查是否可以選擇按鈕。
        /// </summary>
        /// <returns>如果可以選擇按鈕，則為 true；否則為 false。</returns>
        public bool IsCanPick()
        {
            return this.CanPick;
        }
        /// <summary>
        /// 選擇按鈕並觸發選擇事件。
        /// </summary>
        /// <param name="song">要選擇的歌曲標題按鈕</param>
        public void PickCard(SongTitle song)
        {
            if (!CanPick) { return; }   // 如果目前不可選擇按鈕，則直接返回
            this.CanPick = false; // 設置無法再次選擇歌曲
            this.PickSong = song; // 記錄當前選擇的歌曲
            if (PickSong == null) { return; } // 如果未選擇按鈕，則不執行下面的程式碼，直接返回
            bool match = PickSong.File == this.Song; // 檢查是否匹配
            PickSong.Image = SongTitle.ButtonLightImage; // 根據按鈕是否顯示文字設定圖片
            HandleMismatch(PickSong, match); // 處理不匹配的情況
            if (Managerlistener != null) { Managerlistener.PickSongTitle(PickSong, match); } // 調用管理監聽者的方法，通知選擇按鈕事件
        }

        /// <summary>
        /// 高亮顯示不匹配的歌曲標題，並進行相應的處理。
        /// </summary>
        /// <param name="songTitle">要高亮顯示的歌曲標題對象。</param>
        /// <param name="match">歌曲標題對象是否匹配。</param>
        private async void HandleMismatch(SongTitle songTitle, bool match)
        {
            if (match && List.Contains(songTitle)) { List.Remove(songTitle); } // 如果匹配，從管理器中移除該按鈕
            songTitle.Match = match; // 設置歌曲標題的匹配狀態
            songTitle.Selected = true; // 設置歌曲標題為選定狀態
            Image originalImage = songTitle.Image; // 保存原始圖片  
            songTitle.Image = match ? SongTitle.ButtonGreenImage : SongTitle.ButtonRedImage; // 根據是否匹配結果顯示不同圖片
            await Task.Delay(500); // 等待500毫秒
            CanPick = true; // 可以再次選擇歌曲
            songTitle.Enabled = !match; // 根據匹配狀態設置歌曲標題的啟用狀態
            if (!match) { songTitle.Image = originalImage; songTitle.Selected = false; } // 如果不匹配，則恢復原始圖片並取消選定狀態
        }
    }
}

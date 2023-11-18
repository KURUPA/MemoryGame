using System.Data;
using NAudio.Wave;
using Timer = System.Windows.Forms.Timer;

namespace MemoryGame
{
    /// <summary>
    /// 管理和控制遊戲中的歌曲標題按鈕。
    /// </summary>
    public class SongTitleManager
    {
        public List<SongTitle> list { get; set; }
        private string CardTitleFile { get; set; }
        private string song { get; set; }
        public bool CanPick = true;
        public IManagerListener? managerlistener;
        public SongTitle? pickSong;

        /// <summary>
        /// SongTitleManager 類別的構造函數，用於初始化管理歌曲標題按鈕的物件。
        /// </summary>
        public SongTitleManager()
        {
            list = new List<SongTitle>();
            CardTitleFile = "";
            song = "";
        }

        /// <summary>
        /// 將歌曲標題按鈕添加到管理器中。
        /// </summary>
        /// <param name="card">要添加的歌曲標題按鈕</param>
        public void AddSongTitle(SongTitle card)
        {
            list.Add(card);
        }

        /// <summary>
        /// 設定當前播放的歌曲。
        /// </summary>
        /// <param name="File">要設定的歌曲檔案名</param>
        public void setSong(string File)
        {
            this.song = File;
        }

        /// <summary>
        /// 清除管理器中的所有按鈕。
        /// </summary>
        public void ClearAllCard()
        {
            list.Clear();
        }

        /// <summary>
        /// 隨機分配按鈕的對應歌曲。
        /// </summary>
        public void RandomlyAssignKeys()
        {
            // 建立一個亂數產生器，用於產生隨機數值。
            Random random = new Random();
            // 使用亂數重新排列 "list" 中的項目，以實現隨機分配。
            list = this.list.OrderBy(card => random.Next()).ToList();
            // 創建一個新的字串清單 "keys" 來儲存歌曲檔案的識別鍵。
            List<string> keys = new List<string>();
            // 疊代遍歷 MainForm.songDataTable 中的每一行資料。
            foreach (DataRow row in MainForm.songDataTable.Rows)
            {
                // 從每一行中擷取 "File" 欄位的內容轉換為字串。
                string? file = row["File"].ToString();
                // 如果 "file" 不為空，則將其添加到 "keys" 清單中。
                if (file != null)
                {
                    keys.Add(file);
                }
            }
            // 使用亂數重新排列 "keys" 清單中的識別鍵，以實現隨機分配。
            keys = keys.OrderBy(x => random.Next()).ToList();
            // 檢查 "keys" 清單中的元素數是否小於 "list" 清單中的元素數。
            if (keys.Count < list.Count)
            {
                // 如果 "keys" 元素數不足，則輸出訊息並返回。
                Console.WriteLine("not enough File keys: " + keys.ToString());
                return;
            }
            // 遍歷 "list" 清單中的按鈕項目，並分配對應的歌曲識別鍵。
            for (int i = 0; i < list.Count; i++)
            {
                // 從 "keys" 清單中取得一個識別鍵。
                string key = keys[i];
                // 將識別鍵分配給 "list" 清單中的按鈕項目的 "File" 屬性。
                list[i].File = key;
                // 執行按鈕翻轉操作，傳遞 "false" 參數以指示不要執行翻轉動畫。
                list[i].FlipOver(false);
            }
        }


        /// <summary>
        /// 從管理器中隨機獲取一首歌曲。
        /// </summary>
        /// <returns>隨機選取的歌曲檔案名</returns>
        public string getRandomSong()
        {
            string song = list.ElementAt(new Random().Next(list.Count)).File;
            return song;
        }

        /// <summary>
        /// 檢查是否可以選擇按鈕。
        /// </summary>
        /// <returns>如果可以選擇按鈕，則為 true；否則為 false。</returns>
        public Boolean isCanPick()
        {
            return this.CanPick;
        }

        /// <summary>
        /// 選擇按鈕並觸發選擇事件。
        /// </summary>
        /// <param name="song">要選擇的歌曲標題按鈕</param>
        public void PickCard(SongTitle song)
        {
            this.CanPick = false;
            this.pickSong = song;

            if (pickSong == null)
            {
                return; // 如果未選擇按鈕，則不執行下面的程式碼，直接返回
            }

            bool match = pickSong.File == this.song;
            pickSong.FlipOver(false); // 翻轉按鈕，將其蓋上
            pickSong.Image = pickSong.IsShowText ? SongTitle.ButtonLightImage : SongTitle.ButtonImage; // 根據按鈕是否顯示文字設定圖片
            MismatchedEventHandler(pickSong, match);

            CardTitleFile = ""; // 清空按鈕標題文件

            if (managerlistener != null)
            {
                managerlistener.PickSongTitle(pickSong, match); // 調用管理監聽者的方法，通知選擇按鈕事件
            }
        }

        /// <summary>
        /// 高亮顯示不匹配的歌曲標題。
        /// </summary>
        /// <param name="songTitle">要高亮顯示的歌曲標題對象。</param>
        private async void MismatchedEventHandler(SongTitle songTitle, bool match)
        {
            // 播放錯誤提示音效
            using (var audioFile = new AudioFileReader("assets/sound/maou_se_onepoint33.mp3"))
            {
                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(audioFile);
                    outputDevice.Play();
                    // 當音效播放結束後，釋放資源
                    outputDevice.PlaybackStopped += (sender, e) =>
                    {
                        audioFile.Dispose();
                        outputDevice.Dispose();
                    };
                }
            }
            songTitle.PickMatch = match;
            songTitle.Match = false;
            // 保存原始圖像
            Image originalImage = songTitle.Image;
            // 高亮顯示不匹配的歌曲標題
            songTitle.Image = match ? SongTitle.ButtonGreenImage : SongTitle.ButtonRedImage;
            // 等待一段時間以恢復原始圖像
            await Task.Delay(500);
            if (match)
            {
                // songTitle.Visible = false; // 如果選中的按鈕的檔案名與當前播放的歌曲相符，則將按鈕設為不可見
                songTitle.Enabled = false; // 並設為不可選擇
                if (list.Count > 0 && list.Contains(songTitle))
                {
                    list.Remove(songTitle); // 從管理器中移除該按鈕
                }
            }
            else
            {
                songTitle.Image = originalImage;                // 恢復原始圖像

                songTitle.Match = true;

            }
        }
    }
}

namespace MemoryGame.Tabs;

using NAudio.Wave;
using System.Diagnostics;
using System.Windows.Forms;
/// <summary>
/// 遊戲關卡的頁面。
/// </summary>
public class Game : TabPage, IManagerListener
{
    private readonly Random random; // 隨機數產生器，用於生成隨機數
    private SongTitleManager songTitleManager; // 歌曲標題管理器，用於管理歌曲標題的操作
    public TabControl tabControl; // TabControl 控制項，用於顯示不同的遊戲關卡
    private readonly MainMenu form; // 主選單視窗的參考
    private TimeSpan time; // 遊戲時間的時間間隔
    private Label timeboard; // 顯示遊戲時間的標籤控制項
    private PictureBox buttonPlay; // 開始按鈕的圖片按鈕控制項
    private PictureBox buttonRestart; // 重播按鈕的圖片按鈕控制項
    private PictureBox buttonNext; // 下一首按鈕的圖片按鈕控制項
    private Stopwatch stopwatch; // 用於計時的秒表
    private Timer timer; // 用於觸發事件的計時器
    private WaveOut waveOut; // 音訊播放的 WaveOut 控制項

    /// <summary>
    /// Game 類別的構造函數，用於初始化遊戲的頁面。
    /// </summary>
    /// <param name="tabControl">包含遊戲關卡的 TabControl</param>
    /// <param name="form">主選單視窗的參考</param>
    public Game(TabControl tabControl, MainMenu form)
    {
        // 初始化 Level1 的成員變數
        this.tabControl = tabControl;
        this.form = form;
        this.random = new Random();
        waveOut = new WaveOut();
        this.songTitleManager = GenerateAllSongTitle();
        this.songTitleManager.managerlistener = this;
        this.Text = "Level 1";

        // 創建並設定時間顯示的標籤
        this.timeboard = new Label
        {
            Font = MainMenu.GetMicrosoftJhengHeiFont(64),
            Location = new Point(1300, 800),
            ForeColor = Color.White,
            Size = new Size(560, 110),
            Visible = true,
            Text = "時間：00:00"
        };
        this.Controls.Add(this.timeboard);

        // 初始化計時器和秒表
        this.stopwatch = new Stopwatch();
        this.timer = new Timer();
        this.timer.Tick += (sender, eventArgs) => SetTime(stopwatch.Elapsed);

        // 生成並初始化重播按鈕
        this.buttonRestart = GenerateImageButton(0, "Restart", "重播");
        this.buttonRestart.MouseUp += (sender, eventArgs) => PlaySong();
        this.buttonRestart.Enabled = false;
        this.buttonRestart.Visible = false;

        // 生成並初始化下一首按鈕
        this.buttonNext = GenerateImageButton(300, "Next", "下一首");
        this.buttonNext.MouseUp += (sender, eventArgs) => SetNextSong();
        this.buttonNext.Enabled = false;
        this.buttonNext.Visible = false;

        // 生成並初始化開始按鈕
        this.buttonPlay = GenerateImageButton(0, "Play", "開始");
        this.buttonPlay.MouseDown += (sender, eventArgs) =>
        {
            // 開始遊戲，啟用相關按鈕，開始計時器並秒表
            SetNextSong();
            this.buttonRestart.Enabled = true;
            this.buttonRestart.Visible = true;
            this.buttonNext.Enabled = true;
            this.buttonNext.Visible = true;
            if (buttonPlay != null)
            {
                this.buttonPlay.Enabled = false;
                this.buttonPlay.Visible = false;
            }
            this.songTitleManager.list.ForEach((song) => song.Visible = true);
            this.timer.Start();
            this.stopwatch.Start();
        };
    }
    /// <summary>
    /// 生成圖片按鈕(PictureBox)控制項，Y軸預設為720。
    /// </summary>
    /// <param name="x">按鈕的 X 軸位置</param>
    /// <param name="name">按鈕的名稱，用於載入圖片</param>
    /// <param name="text">按鈕下方的文字</param>
    /// <returns>生成的圖片按鈕(PictureBox)</returns>
    private PictureBox GenerateImageButton(int x, string name, string text)
    {
        return GenerateImageButton(x, 720, name, text);
    }

    /// <summary>
    /// 生成一個圖片按鈕(PictureBox)，並在指定位置和文本下創建相關標籤(Label)。
    /// </summary>
    /// <param name="x">按鈕的 X 軸位置</param>
    /// <param name="y">按鈕的 Y 軸位置</param>
    /// <param name="name">按鈕的名稱，用於載入圖片</param>
    /// <param name="text">按鈕下方的文字</param>
    /// <returns>生成的圖片按鈕(PictureBox)</returns>
    private PictureBox GenerateImageButton(int x, int y, string name, string text)
    {
        // 創建圖片按鈕 (PictureBox) 並設定相關屬性
        PictureBox button = new PictureBox
        {
            Size = new Size(160, 160),
            Location = new Point(80 + x, 720),
            SizeMode = PictureBoxSizeMode.Zoom,
            Image = Image.FromFile($"assets/texture/{name}/A_{name}2.png"),
            BackColor = Color.Transparent
        };
        this.Controls.Add(button); // 將按鈕添加到控制項
        // 創建標籤 (Label) 並設定相關屬性
        Label label = new Label
        {
            ForeColor = Color.White,
            Text = text,
            Size = new Size(220, 160),
            Location = new Point(50 + x, button.Location.Y + 120),
            TextAlign = ContentAlignment.MiddleCenter
        };
        this.Controls.Add(label); // 將標籤添加到控制項
        // 處理按鈕的按下事件以切換圖片為按下狀態
        button.MouseDown += (sender, eventArgs) =>
        {
            if (sender is PictureBox pictureBox)
            {
                pictureBox.Image = Image.FromFile($"assets/texture/{name}/A_{name}3.png");
            }
        };
        // 處理按鈕的釋放事件以切換圖片回原始狀態
        button.MouseUp += (sender, eventArgs) =>
        {
            if (sender is PictureBox pictureBox)
            {
                pictureBox.Image = Image.FromFile($"assets/texture/{name}/A_{name}2.png");
            }
        };
        // 處理按鈕的可見性或啟用狀態變化以調整相關標籤的狀態
        button.VisibleChanged += (sender, eventArgs) =>
        {
            label.Visible = button.Visible;
            label.Enabled = button.Enabled;
        };
        return button; // 返回生成的圖片按鈕 (PictureBox)
    }



    /// <summary>
    /// 開始播放音樂。
    /// </summary>
    private void PlaySong()
    {
        // 檢查播放列表是否為空，如果是，則不執行播放操作
        if (songTitleManager.list.Count <= 0)
        {
            return;
        }

        // 從播放列表中隨機選擇一首歌曲
        string song = this.songTitleManager.list[random.Next(this.songTitleManager.list.Count())].File;

        // 設定管理器的當前歌曲
        this.songTitleManager.setSong(song);

        // 創建Mp3FileReader以讀取選定歌曲的MP3文件
        var reader = new Mp3FileReader("assets/song/" + song + ".mp3");

        // 初始化音訊輸出設備並開始播放選定的歌曲
        waveOut.Init(reader);
        waveOut.Play();

        // 在控制台中輸出正在播放的歌曲資訊
        Console.WriteLine("正在播放歌曲：{0}", song);
    }

    /// <summary>
    /// 設定下一首歌曲並播放。
    /// </summary>
    private void SetNextSong()
    {
        try
        {
            // 停止當前播放的歌曲
            waveOut.Stop();

            // 播放下一首歌曲
            PlaySong();
        }
        catch (System.Exception)
        {
            // 捕獲異常，並輸出訊息到控制台，說明播放列表中沒有歌曲
            Console.WriteLine("當前播放列表中沒有任何歌曲。");
        }
    }


    /// <summary>
    /// 生成所有歌曲標題按鈕並配置它們的位置，然後返回相應的歌曲標題管理器。
    /// </summary>
    /// <returns>已配置的歌曲標題管理器。</returns>
    private SongTitleManager GenerateAllSongTitle()
    {
        // 計算要放置歌曲標題按鈕的起始X座標，以便使它們水平居中顯示在TabControl中
        int xxx = (tabControl.Width - (5 * (6 + SongTitle.CARD_WIDTH))) / 2;
        // 創建歌曲標題管理器
        SongTitleManager manager = new SongTitleManager();
        // 遍歷行和列以創建歌曲標題按鈕，將它們添加到控制項中並添加到管理器中
        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                SongTitle card = SongTitle.CreateSongTitle(manager, xxx, 60, col, row, row * 10 + col, "", false, true);
                Controls.Add(card);
                manager.AddSongTitle(card);
            }
        }
        // 隨機分配歌曲標題按鈕的鍵值
        manager.RandomlyAssignKeys();
        return manager;  // 返回已配置的歌曲標題管理器
    }

    /// <summary>
    /// 設定遊戲時間並更新Lable。
    /// </summary>
    /// <param name="time">遊戲經過的時間</param>
    private void SetTime(TimeSpan time)
    {
        // 設定遊戲時間
        this.time = time;
        // 取得經過的總時間
        TimeSpan elapsedTime = this.stopwatch.Elapsed;
        // 更新時間顯示的Lable，以顯示經過的分鐘和秒數
        this.timeboard.Text = $"時間：{elapsedTime.Minutes:D2}:{elapsedTime.Seconds:D2}";
    }

    /// <summary>
    /// 處理歌曲標題挑選事件。
    /// </summary>
    /// <param name="songTitle">選中的歌曲標題</param>
    /// <param name="match">是否匹配</param>
    void IManagerListener.PickSongTitle(SongTitle songTitle, bool match)
    {
        if (match)
        {
            SongTitlePickMatch(songTitle);
        }
        else
        {
            Timer timer = new()
            {
                Interval = 100
            };
            int count = 0;
            Label addTime = new()
            {
                Font = MainMenu.GetMicrosoftJhengHeiFont(48),
                Location = new Point(1300, 800),
                ForeColor = Color.Red,
                Size = new Size(560, 110),
                Visible = true,
                Text = "懲罰加時(+00:01)"
            };
            timer.Tick += (sender, eventArgs) =>
            {
                count++;
                if (count >= 10)
                {
                    timer.Stop();
                    timer.Dispose();
                }
            };
            timer.Start();
        }
    }

    /// <summary>
    /// 處理按鈕挑選匹配事件。
    /// </summary>
    /// <param name="songTitle">選中的歌曲標題</param>
    void SongTitlePickMatch(SongTitle songTitle)
    {
        try
        {
            // 如果管理器中的列表為空，則進行通關處理
            if (songTitleManager.list.Count <= 0)
            {
                string timeText = $"通關時間：{this.time.Minutes:D2}:{this.time.Seconds:D2}";
                this.form.timeboard1.Text = timeText;
                ResetAll();
                tabControl.SelectedIndex = 0;
                return;
            }
            // 執行下一首歌操作
            SetNextSong();
        }
        catch (System.Exception)
        {
            // 發生異常時輸出錯誤資訊到控制台
            Console.WriteLine("WTF Boom");
            // 重新引發異常，以便上層捕獲
            throw;
        }
    }
    /// <summary>
    /// 重置遊戲狀態，初始化遊戲界面和計時器。
    /// </summary>
    public void ResetAll()
    {
        // 清除所有控制項，以便準備重新初始化遊戲界面
        this.Controls.Clear();
        // 生成新的歌曲標題管理器並設置當前的管理聆聽器為本實例
        this.songTitleManager = GenerateAllSongTitle();
        this.songTitleManager.managerlistener = this;
        // 設定遊戲視窗的標題
        this.Text = "Level 1";
        // 創建時間顯示的標籤並設定其屬性
        this.timeboard = new Label
        {
            Font = MainMenu.GetMicrosoftJhengHeiFont(64),
            Location = new Point(1300, 800),
            ForeColor = Color.White,
            Size = new Size(560, 110),
            Visible = true,
            Text = "時間：00:00"
        };
        // 添加時間顯示標籤到遊戲界面的控制項中
        this.Controls.Add(this.timeboard);
        // 初始化計時器和秒表
        this.stopwatch = new Stopwatch();
        this.timer = new Timer();
        // 設定計時器的事件處理函數，用於更新遊戲時間顯示
        this.timer.Tick += (sender, eventArgs) => SetTime(stopwatch.Elapsed);
        // 生成重播按鈕並設定相關屬性和事件處理函數
        this.buttonRestart = GenerateImageButton(200, "Restart", "重播");
        this.buttonRestart.MouseUp += (sender, eventArgs) => PlaySong();
        // 生成下一首按鈕並設定相關屬性和事件處理函數
        this.buttonNext = GenerateImageButton(400, "Next", "下一首");
        this.buttonNext.MouseUp += (sender, eventArgs) => SetNextSong();
        // 隱藏並禁用重播和下一首按鈕
        this.buttonRestart.Enabled = false;
        this.buttonRestart.Visible = false;
        this.buttonNext.Enabled = false;
        this.buttonNext.Visible = false;
        // 生成開始按鈕並設定相關屬性和事件處理函數
        this.buttonPlay = GenerateImageButton(200, "Play", "開始");
        this.buttonPlay.MouseDown += (sender, eventArgs) =>
        {
            // 開始遊戲，啟用相關按鈕，開始計時器並秒表
            SetNextSong();
            this.buttonRestart.Enabled = true;
            this.buttonRestart.Visible = true;
            this.buttonNext.Enabled = true;
            this.buttonNext.Visible = true;
            if (buttonPlay != null)
            {
                this.buttonPlay.Enabled = false;
                this.buttonPlay.Visible = false;
            }
            this.songTitleManager.list.ForEach((song) => song.Visible = true);
            this.timer.Start();
            this.stopwatch.Start();
        };
    }

}
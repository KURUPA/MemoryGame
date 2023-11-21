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
    private readonly SongTitleManager songTitleManager; // 歌曲標題管理器，用於管理歌曲標題的操作
    public TabControl TabControl { get; set; } // TabControl 控制項，用於顯示不同的遊戲關卡
    private readonly MainMenu form; // 主選單視窗的參考
    private TimeSpan Time { get; set; } // 遊戲時間的時間間隔
    private readonly Label timeboard; // 顯示遊戲時間的標籤控制項
    private readonly Stopwatch stopwatch; // 用於計時的秒表
    private readonly Timer timer; // 用於觸發事件的計時器
    private readonly WaveOut waveOut; // 音訊播放的 WaveOut 控制項

    /// <summary>
    /// Game 類別的構造函數，用於初始化遊戲的頁面。
    /// </summary>
    /// <param name="tabControl">包含遊戲關卡的 TabControl</param>
    /// <param name="form">主選單視窗的參考</param>
    public Game(TabControl tabControl, MainMenu form)
    {
        // 初始化成員變數
        this.BackColor = Color.DarkSlateGray;
        this.TabControl = tabControl;
        this.form = form;
        this.random = new Random();
        waveOut = new WaveOut();
        this.songTitleManager = GenerateAllSongTitle();
        this.songTitleManager.Managerlistener = this;
        this.Text = "Game";
        this.timeboard = new Label  // 創建並設定時間顯示的標籤
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
        PictureBox buttonRestart = GenerateImageButton(0, "Restart", "重播");
        buttonRestart.Enabled = false;
        buttonRestart.Visible = false;
        buttonRestart.MouseUp += (sender, eventArgs) => PlaySong();
        // 生成並初始化下一首按鈕
        PictureBox buttonNext = GenerateImageButton(300, "Next", "下一首");
        buttonNext.Enabled = false;
        buttonNext.Visible = false;
        buttonNext.MouseUp += (sender, eventArgs) => NextSong();
        // 生成並初始化開始按鈕
        PictureBox buttonPlay = GenerateImageButton(0, "Play", "開始");
        buttonPlay.MouseDown += (sender, eventArgs) =>
        {
            NextSong();  // 調用 NextSong 方法
            buttonRestart.Enabled = true;  // 啟用 buttonRestart 按鈕
            buttonRestart.Visible = true;  // 顯示 buttonRestart 按鈕
            buttonNext.Enabled = true;  // 啟用 buttonNext 按鈕
            buttonNext.Visible = true;  // 顯示 buttonNext 按鈕
            if (buttonPlay != null) // 檢查 buttonPlay 是否為 null，避免空引用例外
            {
                buttonPlay.Enabled = false;  // 禁用 buttonPlay 按鈕
                buttonPlay.Visible = false;  // 隱藏 buttonPlay 按鈕
            }
            // 對每個歌曲執行 InitializeDisplay 方法
            this.songTitleManager.List.ForEach((song) => song.InitializeDisplay());
            this.timer.Start();  // 開始計時器
            this.stopwatch.Start();  // 開始秒表
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
    /// 生成一個圖片按鈕(PictureBox)，並在指定位置和文字下創建相關標籤(Label)。
    /// </summary>
    /// <param name="x">按鈕的 X 軸位置</param>
    /// <param name="y">按鈕的 Y 軸位置</param>
    /// <param name="name">按鈕的名稱，用於載入圖片</param>
    /// <param name="text">按鈕下方的文字</param>
    /// <returns>生成的圖片按鈕(PictureBox)</returns>
    private PictureBox GenerateImageButton(int x, int y, string name, string text)
    {
        PictureBox button = new() // 創建圖片按鈕 (PictureBox) 並設定相關屬性
        {
            Size = new Size(160, 160),
            Location = new Point(80 + x, 720),
            SizeMode = PictureBoxSizeMode.Zoom,
            Image = Image.FromFile($"assets/texture/{name}/A_{name}2.png"),
            BackColor = Color.Transparent
        };
        this.Controls.Add(button); // 將按鈕添加到控制項
        Label label = new() //創建標籤(Label) 並設定相關屬性
        {
            ForeColor = Color.White,
            Text = text,
            Size = new Size(220, 160),
            Location = new Point(50 + x, button.Location.Y + 120),
            TextAlign = ContentAlignment.MiddleCenter
        };
        this.Controls.Add(label); // 將標籤添加到控制項
        button.MouseDown += (sender, eventArgs) =>  // 處理按鈕的按下事件以切換圖片為按下狀態
        {
            if (sender is PictureBox pictureBox)
            { pictureBox.Image = Image.FromFile($"assets/texture/{name}/A_{name}3.png"); }
        };
        button.MouseUp += (sender, eventArgs) =>    // 處理按鈕的釋放事件以切換圖片回原始狀態
        {
            if (sender is PictureBox pictureBox)
            { pictureBox.Image = Image.FromFile($"assets/texture/{name}/A_{name}2.png"); }
        };
        button.VisibleChanged += (sender, eventArgs) => // 處理按鈕的可見性或啟用狀態變化以調整相關標籤的狀態
        {   // 設定標籤的可見性和啟用狀態與按鈕相同
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
        if (songTitleManager.List.Count <= 0)   // 檢查播放列表是否為空，如果是，則不執行播放操作
        {
            return;
        }
        // 創建Mp3FileReader以讀取選定歌曲的MP3文件
        var reader = new Mp3FileReader("assets/song/" + songTitleManager.Song + ".mp3");
        // 初始化音訊輸出設備並開始播放選定的歌曲
        waveOut.Init(reader);
        waveOut.Play();
        // 在控制台中輸出正在播放的歌曲資訊
        Console.WriteLine("正在播放歌曲：{0}\t\t 剩餘歌曲：{1}", songTitleManager.Song, songTitleManager.List.Count);
    }

    /// <summary>
    /// 設定下一首歌曲並播放。
    /// </summary>
    private void NextSong()
    {
        if (songTitleManager.List.Count <= 0) // 檢查播放列表是否為空，如果是，則不執行播放操作
        {
            return;
        }
        // 從播放列表中隨機選擇一首歌曲
        string song = this.songTitleManager.List[random.Next(this.songTitleManager.List.Count())].File;
        this.songTitleManager.Song = song;  // 設定管理器的當前歌曲
        waveOut.Stop(); // 停止當前播放的歌曲
        PlaySong(); // 播放下一首歌曲
    }


    /// <summary>
    /// 生成所有歌曲標題按鈕並配置它們的位置，然後返回相應的歌曲標題管理器。
    /// </summary>
    /// <returns>已配置的歌曲標題管理器。</returns>
    private SongTitleManager GenerateAllSongTitle()
    {
        // 計算要放置歌曲標題按鈕的起始X座標，以便使它們水平居中顯示在TabControl中
        int xPos = (TabControl.Width - (5 * (6 + SongTitle.CARD_WIDTH))) / 2;
        SongTitleManager manager = new();  // 創建歌曲標題管理器
        for (int row = 0; row < 4; row++)    // 遍歷行和列以創建歌曲標題按鈕，將它們添加到控制項中並添加到管理器中
        {
            for (int col = 0; col < 5; col++)
            {
                SongTitle songTitle = SongTitle.Create(manager, xPos, 60, col, row, row * 10 + col, "");  // 創建歌曲標題按鈕並設定相關屬性
                songTitle.Visible = false;  // 設置歌曲標題按鈕的可見性為false
                Controls.Add(songTitle);  // 將歌曲標題按鈕添加到控制項
                manager.List.Add(songTitle);  // 將歌曲標題按鈕添加到管理器的列表中
            }
        }
        manager.RandomlyAssignKeys();   // 隨機分配歌曲標題按鈕的鍵值
        return manager;  // 返回已配置的歌曲標題管理器
    }

    /// <summary>
    /// 設定遊戲時間並更新Lable。
    /// </summary>
    /// <param name="time">遊戲經過的時間</param>
    private void SetTime(TimeSpan time)
    {
        this.Time = time;   // 設定遊戲時間
        TimeSpan elapsedTime = this.stopwatch.Elapsed;  // 取得經過的總時間
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
        if (match)  // 如果歌曲標題匹配
        {
            if (songTitleManager.List.Count <= 0)  // 如果管理器中的列表為空，表示已通關
            {
                string timeText = $"通關時間：{this.Time.Minutes:D2}:{this.Time.Seconds:D2}";  // 計算通關時間
                this.form.Timeboard.Text = timeText;    // 在主選單視窗上更新通關時間顯示
                TabControl.SelectedIndex = 0;   // 切換至主選單視窗
                return; //中斷之後操作
            }
            NextSong(); // 執行下一首歌操作
        }
    }
}
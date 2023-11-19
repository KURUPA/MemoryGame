namespace MemoryGame.Tabs
{
    /// <summary>
    /// 主選單 TabPage，用於顯示遊戲的主選單和選項。
    /// </summary>
    public class MainMenu : TabPage
    {
        private Button ButtonStart { get; set; } //開始按鈕
        public Label Timeboard { get; set; } //關卡時間分數
        public TabControl TabControl { get; set; } //標籤控制 
        private Label Title { get; set; } //標題

        /// <summary>
        /// MainMenu 類別的構造函數，用於初始化主選單 TabPage。
        /// </summary>
        /// <param name="tabControl">包含主選單的 TabControl</param>
        public MainMenu(TabControl tabControl)
        {
            BackColor = Color.DarkSlateGray;
            Text = "MainMenu"; // 設定主選單視窗的標題
            BorderStyle = BorderStyle.None; // 設定視窗邊框樣式
            this.TabControl = tabControl; // 指定主選單的 TabControl
            SuspendLayout(); // 暫停控制項的佈局邏輯
            this.Title = new Label  // 創建主選單的標題標籤
            {
                Size = new Size(480, 120),
                Location = new Point((tabControl.Width - 480) / 2, (int)(tabControl.Height * 0.2)),
                Text = "延緩失智遊戲",
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.White,
            };
            this.ButtonStart = new Button   // 創建主選單的開始遊戲按鈕
            {
                Name = "buttonStart1",
                Size = new Size(300, 90),
                TabIndex = 0,
                Text = "開始遊戲",
                TextAlign = ContentAlignment.MiddleCenter,
                UseVisualStyleBackColor = true,
                Location = new Point((tabControl.Width - 300) / 2, (int)(tabControl.Height * 0.5))
            };
            this.ButtonStart.Click += (sender, args) => // 設定開始遊戲按鈕的點擊事件處理程序
            {
                if (tabControl.TabPages.Count > 2) { tabControl.TabPages.RemoveAt(2); } // 刪除索引為2的頁面
                tabControl.TabPages.Insert(2, new Game(tabControl, this)); // 在索引2的位置插入新的Game頁面
                tabControl.SelectedIndex = 1; // 切換至說明頁面
            };
            this.Timeboard = new Label // 創建主選單的時間標籤
            {
                Location = new Point((tabControl.Width - 300) / 2 + 300, (int)(tabControl.Height * 0.5) + 25),
                ForeColor = Color.White,
                Size = new Size(560, 90),
                Visible = true,
                Text = ""
            };
            this.Controls.Add(Timeboard); // 將時間標籤加入主選單
            this.Controls.Add(Title); // 將標題標籤加入主選單
            this.Controls.Add(ButtonStart); // 將開始遊戲按鈕加入主選單
            ResumeLayout(); // 恢復控制項的佈局邏輯
        }

        /// <summary>
        /// 取得用於主選單的標準字型，字型大小36。
        /// </summary>
        public static Font GetMicrosoftJhengHeiFont()
        {
            return GetMicrosoftJhengHeiFont(36);
        }

        /// <summary>
        /// 取得指定大小的主選單標準字型。
        /// </summary>
        /// <param name="Size">字型大小</param>
        public static Font GetMicrosoftJhengHeiFont(int Size)
        {
            Font font = new("Microsoft JhengHei UI", Size, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            return font;
        }
    }
}

namespace MemoryGame.Tabs
{
    using System.Drawing.Text;

    /// <summary>
    /// 主選單 TabPage，用於顯示遊戲的主選單和選項。
    /// </summary>
    public class MainMenu : TabPage
    {
        private Button buttonStart1; //第一關按鈕
        public Label timeboard1; //第一關時間分數
        public TabControl tabControl; //標籤控制 
        private Label Title; //標題

        /// <summary>
        /// MainMenu 類別的構造函數，用於初始化主選單 TabPage。
        /// </summary>
        /// <param name="tabControl">包含主選單的 TabControl</param>
        public MainMenu(TabControl tabControl)
        {
            this.Text = "MainMenu";
            this.BorderStyle = BorderStyle.None;
            this.tabControl = tabControl;
            SuspendLayout();
            // 創建並設定主選單的標題標籤
            this.Title = new Label
            {
                Size = new Size(480, 120),
                Location = new Point((tabControl.Width - 480) / 2, (int)(tabControl.Height * 0.2)),
                Text = "延緩失智遊戲",
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.White,
            };
            // 創建並設定開始遊戲按鈕
            this.buttonStart1 = new Button
            {
                Name = "buttonStart1",
                Size = new Size(300, 90),
                TabIndex = 0,
                Text = "開始遊戲",
                TextAlign = ContentAlignment.MiddleCenter,
                UseVisualStyleBackColor = true,
                Location = new Point((tabControl.Width - 300) / 2, (int)(tabControl.Height * 0.5))
            };
            this.buttonStart1.Click += (sender, args) => tabControl.SelectedIndex = 1;
            // 創建並設定時間顯示的標籤
            this.timeboard1 = new Label
            {
                Location = new Point((tabControl.Width - 300) / 2 + 300, (int)(tabControl.Height * 0.5) + 25),
                ForeColor = Color.White,
                Size = new Size(560, 90),
                Visible = true,
                Text = ""
            };
            this.Controls.Add(timeboard1); 
            this.Controls.Add(Title);
            this.Controls.Add(buttonStart1);
            ResumeLayout();
        }

        /// <summary>
        /// 取得用於主選單的標準字型。
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
            Font font = new System.Drawing.Font("Microsoft JhengHei UI", Size, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            return font;
        }
    }
}

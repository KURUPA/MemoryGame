namespace MemoryGame
{
    /// <summary>
    /// 歌曲標題按鈕。
    /// </summary>
    public class SongTitle : PictureBox
    {
        public readonly SongTitleManager CardManager; // 歌曲標題管理器
        public static readonly Image ButtonLightImage = Image.FromFile("assets/texture/Song_Title/button_light.png"); // 高亮按鈕圖片
        public static readonly Image ButtonLightDownImage = Image.FromFile("assets/texture/Song_Title/button_light_down.png"); // 高亮按鈕按下圖片
        public static readonly Image ButtonRedImage = Image.FromFile("assets/texture/Song_Title/button_red.png"); // 紅色按鈕圖片
        public static readonly Image ButtonGreenImage = Image.FromFile("assets/texture/Song_Title/button_green.png"); // 綠色按鈕圖片
        public string File { get; set; } // 歌曲檔案名
        private string Title { get; set; } // 歌曲標題
        private string Singer { get; set; } // 歌手名稱
        private int YOffset { get; set; } // Y 偏移量
        public const int CARD_WIDTH = 288; // 卡片寬度
        public const int CARD_HEIGHT = 144;  // 卡片高度
        public bool Selected { get; set; }  //選擇被選中
        public bool Match { get; set; }  //選擇是否正確

        /// <summary>
        /// 創建一個新的歌曲標題（SongTitle）對象。
        /// </summary>
        /// <param name="x">初始化 X 坐標。</param>
        /// <param name="y">初始化 Y 坐標。</param>
        /// <param name="cardManager">歌曲標題管理器（SongTitleManager）的引用。</param>
        /// <param name="file">歌曲檔案名。</param>
        /// <param name="index">歌曲標題的索引。</param>
        public SongTitle(int x, int y, SongTitleManager cardManager, String file, int index) : this(x, y, cardManager)
        {
            this.File = file;
        }
        /// <summary>
        /// 創建一個新的歌曲標題（SongTitle）對象。
        /// </summary>
        /// <param name="x">X 坐標。</param>
        /// <param name="y">Y 坐標。</param>
        /// <param name="cardManager">歌曲標題管理器（SongTitleManager）的引用。</param>
        public SongTitle(int x, int y, SongTitleManager cardManager) : this(cardManager)
        {
            this.Location = new Point(x, y);
        }
        /// <summary>
        /// 創建一個新的歌曲標題（SongTitle）對象。
        /// </summary>
        /// <param name="cardManager">歌曲標題管理器（SongTitleManager）的引用。</param>
        public SongTitle(SongTitleManager cardManager)
        {
            this.Title = "";
            this.Singer = "";
            this.File = "";
            this.CardManager = cardManager;
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Size = new Size(CARD_WIDTH, CARD_HEIGHT);
            this.Image = ButtonLightImage;
            this.BackColor = Color.Transparent;
            this.YOffset = 16;
            this.MouseDown += new MouseEventHandler(SongTitleMouseDownEvent);
            this.MouseUp += new MouseEventHandler(SongTitleMouseUpEvent);
            this.Selected = false;
            this.Match = false;
        }
        /// <summary>
        /// 覆蓋控制項的繪製方法，用於自訂繪製標題和歌手文字。
        /// </summary>
        /// <param name="e">包含繪圖資訊的 PaintEventArgs 對象。</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);    // 調用基類的 OnPaint 方法，進行基本的繪製操作
            if (Selected)  // 判斷被選中
            {
                Font font = MemoryGame.Tabs.MainMenu.GetMicrosoftJhengHeiFont(32);  // 使用 Microsoft JhengHei 字體大小為32
                Brush brush = new SolidBrush(Match ? Color.LightGreen : Color.LightPink);   // 根據是否選擇正確而選擇不同的顏色筆刷
                string Text = Match ? "選擇正確" : "選擇錯誤";  // 根據是否選擇正確設置文字內容
                SizeF Size = TextRenderer.MeasureText(Text, font);    // 測量文字的大小 
                PointF Point = new((this.Width - Size.Width) / 2, (this.Height - Size.Height) / 2 - this.YOffset);   // 設置繪製文字的位置，使其居中   
                e.Graphics.DrawString(Text, font, brush, Point);    // 使用 Graphics 對象繪製文字
            }
            else
            {
                Font font = MemoryGame.Tabs.MainMenu.GetMicrosoftJhengHeiFont(24);  // 使用 Microsoft JhengHei 字體大小為24
                Brush brush = new SolidBrush(Color.DarkBlue);   // 使用深藍色畫刷
                SizeF titleSize = TextRenderer.MeasureText(this.Title, font); // 測量標題文字的大小
                SizeF singerSize = TextRenderer.MeasureText(this.Singer, font); // 測量歌手文字的大小
                PointF titlePoint = new((this.Width - titleSize.Width) / 2, (this.Height - titleSize.Height) / 2 - this.YOffset);    // 設置繪製文字的位置，使其居中
                PointF singerPoint = new((this.Width - singerSize.Width) / 2, (this.Height + singerSize.Height) / 2 - this.YOffset); // 設置繪製文字的位置，使其居中
                e.Graphics.DrawString(this.Title, font, brush, titlePoint); // 使用 Graphics 對象繪製標題文字
                e.Graphics.DrawString(this.Singer, font, brush, singerPoint);   // 使用 Graphics 對象繪製歌手文字
            }
        }
        /// <summary>
        /// 滑鼠按下事件處理程序。
        /// </summary>
        /// <param name="sender">事件的源對象。</param>
        /// <param name="e">包含事件數據的 MouseEventArgs 對象。</param>
        private void SongTitleMouseDownEvent(object? sender, MouseEventArgs e)
        {

            if (!CardManager.CanPick) //如果管理器目前不可選擇則阻止處理
            {
                return;
            }
            YOffset = 13;  // 設置 Y 偏移量為 13，改變顯示效果
            Image = ButtonLightDownImage; //圖片改變為被按下的圖片

        }
        /// <summary>
        /// 滑鼠抬起事件處理程序。
        /// </summary>
        /// <param name="sender">事件的源對象。</param>
        /// <param name="e">包含事件數據的 MouseEventArgs 對象。</param>
        private void SongTitleMouseUpEvent(object? sender, MouseEventArgs e)
        {

            if (!CardManager.CanPick) //如果管理器目前不可選擇則阻止處理
            {
                return;
            }
            YOffset = 16; // 恢復 Y 偏移量為 16，恢復顯示效果
            Image = ButtonLightImage; // 將圖片設置為正常狀態
            if (CardManager.List.Count <= 0) // 如果歌曲標題管理器中的卡片數量小於等於 0，則不執行後續操作，避免造成錯誤
            {
                return;
            }
            Enabled = false; //暫時禁用按鈕避免重複點擊
            CardManager.PickCard(this); // 請歌曲標題管理器處理卡片選擇事件
        }
        /// <summary>
        /// 創建一個新的歌曲標題（SongTitle）對象。
        /// </summary>
        /// <param name="cardManager">歌曲標題管理器（SongTitleManager）的引用。</param>
        /// <param name="initX">初始化 X 坐標。</param>
        /// <param name="initY">初始化 Y 坐標。</param>
        /// <param name="x">X 方向上的偏移量。</param>
        /// <param name="y">Y 方向上的偏移量。</param>
        /// <param name="index">歌曲標題的索引。</param>
        /// <param name="key">歌曲標題的標識鍵。</param>
        /// <param name="visible">指定歌曲標題是否可見。</param>
        /// <param name="showText">指定是否顯示文字。</param>
        /// <returns>新創建的 SongTitle 對象。</returns>
        public static SongTitle Create(SongTitleManager cardManager, int initX, int initY, int x, int y, int index, String key)
        {
            SongTitle songTitle = new(    // 創建新的 SongTitle 對象，並設置初始位置
                initX + x * (6 + CARD_WIDTH),
                initY + y * (6 + CARD_HEIGHT),
                cardManager, key, index);
            return songTitle;   // 返回新創建的 SongTitle 對象
        }
        /// <summary>
        /// 初始化按鈕顯示與資訊
        /// </summary>
        public void InitializeDisplay()
        {
            this.Visible = true; // 設定按鈕可見性為 true
            this.Title = MainForm.GetSongInfoByKey(this.File, "Title"); // 獲取並設定標題
            this.Singer = MainForm.GetSongInfoByKey(this.File, "Singer"); // 獲取並設定歌手
        }
    }
}

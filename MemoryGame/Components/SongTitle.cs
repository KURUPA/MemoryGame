namespace MemoryGame
{
    public class SongTitle : PictureBox
    {
        public readonly SongTitleManager CardManager; // 歌曲標題管理器
        public static readonly Image ButtonImage = Image.FromFile("assets/texture/Song_Title/button.png"); // 按鈕圖像
        public static readonly Image ButtonDownImage = Image.FromFile("assets/texture/Song_Title/button_down.png"); // 按鈕按下圖像
        public static readonly Image ButtonLightImage = Image.FromFile("assets/texture/Song_Title/button_light.png"); // 高亮按鈕圖像
        public static readonly Image ButtonLightDownImage = Image.FromFile("assets/texture/Song_Title/button_light_down.png"); // 高亮按鈕按下圖像
        public static readonly Image ButtonRedImage = Image.FromFile("assets/texture/Song_Title/button_red.png"); // 紅色按鈕圖像
        public static readonly Image ButtonGreenImage = Image.FromFile("assets/texture/Song_Title/button_green.png"); // 綠色按鈕圖像
        private bool IsFlipped { get; set; } // 卡片是否翻轉
        public bool IsShowText { get; set; } // 是否顯示文字
        public String File { get; set; } // 歌曲檔案名
        private string Title { get; set; } // 歌曲標題
        private string Singer { get; set; } // 歌手名稱
        private int YOffset { get; set; } // Y 偏移量
        public static readonly int CARD_WIDTH = 288; // 卡片寬度
        public static readonly int CARD_HEIGHT = 144; // 卡片高度
        private float posX; // X 坐標
        private float posY; // Y 坐標
        public bool Match; //選擇是否匹配
        public bool PickMatch; //選擇是否正確


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
            this.Image = ButtonImage;
            this.IsFlipped = false;
            this.BackColor = Color.Transparent;
            this.YOffset = 16;
            this.MouseDown += new MouseEventHandler(SongTitleMouseDownEvent);
            this.MouseUp += new MouseEventHandler(SongTitleMouseUpEvent);
            this.Match = true;
            this.PickMatch = false;
        }

        /// <summary>
        /// 設置是否顯示文字。
        /// </summary>
        /// <param name="isShow">指定是否顯示文字。</param>
        public void setIsShowText(bool isShow)
        {
            this.IsShowText = isShow;
            this.Image = isShow ? ButtonLightImage : ButtonImage;
        }

        /// <summary>
        /// 翻轉歌曲標題卡片，顯示或隱藏歌曲資訊。
        /// </summary>
        /// <param name="flip">指定是否翻轉。</param>
        public void FlipOver(bool flip)
        {
            bool show = flip || this.IsShowText;
            this.Title = show ? MainForm.FindTextByKeyAndType(this.File, "Title") : " ";
            this.Singer = show ? MainForm.FindTextByKeyAndType(this.File, "Singer") : " ";
            IsFlipped = flip;
        }

        /// <summary>
        /// 覆蓋控制項的繪製方法，用於自訂繪製標題和歌手文字。
        /// </summary>
        /// <param name="e">包含繪圖資訊的 PaintEventArgs 對象。</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);    // 調用基類的 OnPaint 方法，進行基本的繪製操作
            // 判斷是否需要顯示文字或卡片已翻轉並且標題不為空
            if (this.IsShowText || this.IsFlipped && !string.IsNullOrEmpty(this.Title))
            {
                if (Match)  // 判斷是否匹配
                {
                    Font font = MemoryGame.Tabs.MainMenu.GetMicrosoftJhengHeiFont(24);  // 使用 Microsoft JhengHei 字體大小為24
                    Brush brush = new SolidBrush(Color.DarkBlue);   // 使用深藍色畫刷
                    SizeF titleSize = TextRenderer.MeasureText(this.Title, font); // 測量標題文字的大小
                    SizeF singerSize = TextRenderer.MeasureText(this.Singer, font); // 測量歌手文字的大小
                    PointF titlePoint = new PointF((this.Width - titleSize.Width) / 2, (this.Height - titleSize.Height) / 2 - this.YOffset);    // 設置繪製文字的位置，使其居中
                    PointF singerPoint = new PointF((this.Width - singerSize.Width) / 2, (this.Height + singerSize.Height) / 2 - this.YOffset); // 設置繪製文字的位置，使其居中
                    e.Graphics.DrawString(this.Title, font, brush, titlePoint); // 使用 Graphics 對象繪製標題文字
                    e.Graphics.DrawString(this.Singer, font, brush, singerPoint);   // 使用 Graphics 對象繪製歌手文字
                }
                else
                {
                    Font font = MemoryGame.Tabs.MainMenu.GetMicrosoftJhengHeiFont(32);  // 使用 Microsoft JhengHei 字體大小為32
                    Brush brush = new SolidBrush(PickMatch ? Color.LightGreen : Color.LightPink);   // 根據是否選擇正確而選擇不同的顏色筆刷
                    string Text = PickMatch ? "選擇正確" : "選擇錯誤";  // 根據是否選擇正確設置文字內容
                    SizeF Size = TextRenderer.MeasureText(Text, font);    // 測量文字的大小 
                    PointF Point = new PointF((this.Width - Size.Width) / 2, (this.Height - Size.Height) / 2 - this.YOffset);   // 設置繪製文字的位置，使其居中   
                    e.Graphics.DrawString(Text, font, brush, Point);    // 使用 Graphics 對象繪製文字
                }
            }

        }

        /// <summary>
        /// 滑鼠按下事件處理程序。
        /// </summary>
        /// <param name="sender">事件的源對象。</param>
        /// <param name="e">包含事件數據的 MouseEventArgs 對象。</param>
        private void SongTitleMouseDownEvent(object? sender, MouseEventArgs e)
        {
            this.YOffset = 13;  // 設置 Y 偏移量為 13，改變顯示效果
            if (this.IsFlipped) // 如果卡片已經翻轉，則不執行後續操作
            {
                return;
            }
            if (this.Image == ButtonImage)  // 根據不同的圖像狀態設置按鈕的圖像
            {
                this.Image = ButtonDownImage;
            }
            else if (this.Image == ButtonLightImage)
            {
                this.Image = ButtonLightDownImage;
            }
        }

        /// <summary>
        /// 滑鼠抬起事件處理程序。
        /// </summary>
        /// <param name="sender">事件的源對象。</param>
        /// <param name="e">包含事件數據的 MouseEventArgs 對象。</param>
        private void SongTitleMouseUpEvent(object? sender, MouseEventArgs e)
        {
            YOffset = 16; // 恢復 Y 偏移量為 16，恢復顯示效果
            Image = ButtonLightImage; // 將圖像設置為正常狀態
            if (CardManager.list.Count <= 0) // 如果歌曲標題管理器中的卡片數量小於等於 0，則不執行後續操作，避免造成錯誤
            {
                return;
            }
            if (!IsShowText)    // 如果未顯示文字，則翻轉卡片顯示文字
            {
                FlipOver(true);
            }
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
        public static SongTitle CreateSongTitle(SongTitleManager cardManager, int initX, int initY, int x, int y, int index, String key, bool visible, bool showText)
        {
            // 創建新的 SongTitle 對象，並設置初始位置
            SongTitle songTitle = new SongTitle(
                initX + x * (6 + CARD_WIDTH),
                initY + y * (6 + CARD_HEIGHT),
                cardManager, key, index);
            songTitle.setIsShowText(showText);  // 設置是否顯示文本
            songTitle.Visible = visible;    // 設置可見性
            songTitle.SetPos(1.0F - 1.0F / x, 1.0F - 1.0F / y); // 設置位置坐標      
            return songTitle;   // 返回新創建的 SongTitle 對象
        }

        /// <summary>
        /// 設置位置坐標。
        /// </summary>
        /// <param name="x">X 坐標。</param>
        /// <param name="y">Y 坐標。</param>
        public void SetPos(float x, float y)
        {
            posX = x;
            posY = y;
        }
    }
}

namespace MemoryGame
{
    public class SongTitle : PictureBox
    {
        public readonly SongTitleManager CardManager;
        public static readonly Image ButtonImage = Image.FromFile("assets/texture/Song_Title/button.png");
        public static readonly Image ButtonDownImage = Image.FromFile("assets/texture/Song_Title/button_down.png");
        public static readonly Image ButtonLightImage = Image.FromFile("assets/texture/Song_Title/button_light.png");
        public static readonly Image ButtonLightDownImage = Image.FromFile("assets/texture/Song_Title/button_light_down.png");
        private bool IsFlipped { get; set; }
        public bool IsShowText { get; set; }
        public String File { get; set; }
        private string Title { get; set; }
        private string Singer { get; set; }
        private int YOffset { get; set; }
        public static readonly int CARD_WIDTH = 288;
        public static readonly int CARD_HEIGHT = 144;
        private float posX;
        private float posY;

        public SongTitle(int x, int y, SongTitleManager cardManager, String file, int index) : this(x, y, cardManager)
        {
            this.File = file;
        }

        public SongTitle(int x, int y, SongTitleManager cardManager) : this(cardManager)
        {
            this.Location = new Point(x, y);
        }

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
        }

        public void setIsShowText(bool IsShow)
        {
            this.IsShowText = IsShow;
            this.Image = IsShow ? ButtonLightImage : ButtonImage;
        }

        public void FlipOver(Boolean flipp)
        {
            bool show = flipp || this.IsShowText;
            this.Title = show ? MainForm.FindTextByKeyAndType(this.File, "Title") : " ";
            this.Singer = show ? MainForm.FindTextByKeyAndType(this.File, "Singer") : " ";
            IsFlipped = flipp;
        }

        public void FlipOver()
        {
            this.FlipOver(!IsFlipped);
        }

        public bool Flipped
        {
            get { return IsFlipped; }
            set
            {
                if (IsFlipped != value)
                {
                    IsFlipped = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// 覆寫了控制項的繪製方法，用於自定義繪製標題和歌手文字。
        /// </summary>
        /// <param name="e">包含繪圖資訊的PaintEventArgs對象。</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);  // 呼叫基類的OnPaint方法以確保正確的繪製行為

            if (this.IsShowText || this.IsFlipped && !string.IsNullOrEmpty(this.Title))
            {
                // 創建字體和筆刷以設置繪製文本的外觀
                Font font = MemoryGame.Tabs.MainMenu.getCubicFont(24);  // 使用24pt的Cubic字體
                Brush brush = new SolidBrush(Color.DarkBlue);  // 使用深藍色筆刷

                // 測量標題和歌手文字的大小
                SizeF titleSize = TextRenderer.MeasureText(this.Title, font);
                SizeF singerSize = TextRenderer.MeasureText(this.Singer, font);

                // 設置繪製文本的位置，使其置中
                PointF titlePoint = new PointF((this.Width - titleSize.Width) / 2, (this.Height - titleSize.Height) / 2 - this.YOffset);
                PointF singerPoint = new PointF((this.Width - singerSize.Width) / 2, (this.Height + singerSize.Height) / 2 - this.YOffset);

                // 使用Graphics對象繪製標題和歌手文字
                e.Graphics.DrawString(this.Title, font, brush, titlePoint);
                e.Graphics.DrawString(this.Singer, font, brush, singerPoint);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SongTitleMouseDownEvent(object? sender, MouseEventArgs e)
        {
            this.YOffset = 13;
            if (!this.CardManager.isCanPick() || this.IsFlipped)
            {
                return;
            }
            if (this.Image == ButtonImage)
            {
                this.Image = ButtonDownImage;
            }
            else if (this.Image == ButtonLightImage)
            {
                this.Image = ButtonLightDownImage;
            }
        }
        /// <summary>
        /// 滑鼠按下鬆開後彈起事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SongTitleMouseUpEvent(object? sender, MouseEventArgs e)
        {
            YOffset = 16;
            Image = ButtonLightImage;
            if (CardManager.list.Count <= 0)
            {
                return;
            }
            if (!CardManager.isCanPick())
            {
                return;
            }
            if (!IsShowText)
            {
                FlipOver(true);
            }
            CardManager.PickCard(this);
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
        /// <param name="showText">指定是否顯示文本。</param>
        /// <returns>新創建的 SongTitle 對象。</returns>
        public static SongTitle CreateSongTitle(SongTitleManager cardManager, int initX, int initY, int x, int y, int index, String key, bool visible, bool showText)
        {
            SongTitle songTitle = new SongTitle(
                initX + x * (6 + CARD_WIDTH),
                initY + y * (6 + CARD_HEIGHT),
                cardManager, key, index);
            songTitle.setIsShowText(showText);
            songTitle.Visible = visible;
            songTitle.SetPos(1.0F - 1.0F / x, 1.0F - 1.0F / y);
            return songTitle;
        }
        /// <summary>
        /// 設置位置座標
        /// </summary>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        public void SetPos(float x, float y)
        {
            posX = x;
            posY = y;
        }
    }
}
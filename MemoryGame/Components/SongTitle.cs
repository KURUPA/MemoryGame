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
        public static readonly int CARD_WIDTH = 144;
        public static readonly int CARD_HEIGHT = 72;

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
            this.MouseDown += new MouseEventHandler(SongTitleMouseDown);
            this.MouseUp += new MouseEventHandler(SongTitleMouseUp);
        }

        public void setIsShowText(bool IsShow)
        {
            this.IsShowText = IsShow;
            this.Image = IsShow ? ButtonLightImage : ButtonImage;
        }

        public void FlipOver(Boolean flipp)
        {
            bool show = flipp || this.IsShowText;
            this.Title = show ? MainForm.FindTextByKeyAndLang(MainForm.LangDataTable, this.File, "Title") : " ";
            this.Singer = show ? MainForm.FindTextByKeyAndLang(MainForm.LangDataTable, this.File, "Singer") : " ";
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

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (this.IsShowText || this.IsFlipped && !string.IsNullOrEmpty(this.Title))
            {
                Font font = MemoryGame.Tabs.MainMenu.getCubicFont();
                Brush brush = new SolidBrush(Color.DarkBlue);
                SizeF titleSize = TextRenderer.MeasureText(this.Title, font);
                SizeF singerSize = TextRenderer.MeasureText(this.Singer, font);
                PointF titlePoint = new PointF((this.Width - titleSize.Width) / 2, (this.Height - titleSize.Height) / 2 - this.YOffset);
                PointF singerPoint = new PointF((this.Width - singerSize.Width) / 2, (this.Height + singerSize.Height) / 2 - this.YOffset);
                e.Graphics.DrawString(this.Title, font, brush, titlePoint);
                e.Graphics.DrawString(this.Singer, font, brush, singerPoint);
            }
        }

        private void SongTitleMouseDown(object? sender, MouseEventArgs e)
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

        private void SongTitleMouseUp(object? sender, MouseEventArgs e)
        {
            this.YOffset = 16;
            this.Image = ButtonLightImage;
            if (!this.CardManager.isCanPick())
            {
                return;
            }
            if (!this.IsShowText)
            {
                this.FlipOver(true);
            }
            this.CardManager.PickCard(this);
        }
    }
}
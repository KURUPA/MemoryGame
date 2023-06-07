using System.Drawing;

namespace MemoryGame
{
    public class SongTitle : PictureBox
    {
        public readonly CardManager CardManager;
        public readonly Image ButtonImage = Image.FromFile("assets/texture/Song_Title/button.png");
        public readonly Image ButtonDownImage = Image.FromFile("assets/texture/Song_Title/button_down.png");
        public readonly Image ButtonUpImage = Image.FromFile("assets/texture/Song_Title/button_up.png");
        private bool IsFlipped { get; set; }

        public String File { get; set; }
        public int Id { get; set; }

        private string Title { get; set; }
        private string Singer { get; set; }
        public static readonly int CARD_WIDTH = 144;
        public static readonly int CARD_HEIGHT = 72;

        public SongTitle(int x, int y, CardManager cardManager, String key, int index) : this(x, y, cardManager)
        {
            this.File = key;
            this.Id = index;
        }

        public SongTitle(int x, int y, CardManager cardManager) : this(cardManager)
        {
            this.Location = new Point(x, y);
        }

        public SongTitle(CardManager cardManager)
        {
            this.Title = "";
            this.Singer = "";
            this.File = "";
            this.CardManager = cardManager;
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Size = new Size(CARD_WIDTH, CARD_HEIGHT);
            this.Image = this.ButtonImage;
            this.IsFlipped = false;
            this.MouseDown += new MouseEventHandler(SongTitleMouseDown);
            this.MouseUp += new MouseEventHandler(SongTitleMouseUp);
        }

        public void FlipOver(Boolean flipp)
        {
            this.Image = flipp ? this.ButtonUpImage : this.ButtonImage;
            this.Title = flipp ? MainForm.FindTextByKeyAndLang(MainForm.LangDataTable, this.File, "Title") : " ";
            this.Singer = flipp ? MainForm.FindTextByKeyAndLang(MainForm.LangDataTable, this.File, "Singer") : " ";
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

            if (IsFlipped && !string.IsNullOrEmpty(Title))
            {
                Font font = MemoryGame.Tabs.MainMenu.getFont();
                Brush brush = new SolidBrush(Color.DarkBlue);
                SizeF titleSize = TextRenderer.MeasureText(Title, font);
                SizeF singerSize = TextRenderer.MeasureText(Singer, font);
                PointF titlePoint = new PointF((this.Width - titleSize.Width) / 2, (this.Height - titleSize.Height) / 2 - 16);
                PointF singerPoint = new PointF((this.Width - singerSize.Width) / 2, (this.Height + singerSize.Height) / 2 - 16);
                e.Graphics.DrawString(Title, font, brush, titlePoint);
                e.Graphics.DrawString(Singer, font, brush, singerPoint);
            }
        }


        private void SongTitleMouseDown(object? sender, MouseEventArgs e)
        {
            if (!CardManager.isCanPick() || this.IsFlipped)
            {
                return;
            }
            this.Image = this.ButtonDownImage;
        }
        private void SongTitleMouseUp(object? sender, MouseEventArgs e)
        {
            this.Image = this.ButtonUpImage;
            if (!CardManager.isCanPick())
            {
                return;
            }
            this.FlipOver(true);
            CardManager.PickCard(this);

        }

    }

}
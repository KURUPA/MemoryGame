using System.Drawing;

namespace MemoryGame
{
    public class SongTitle : PictureBox
    {
        public readonly CardManager CardManager;
        public readonly Image FrontImage;
        public readonly Image BackImage;
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
            this.FrontImage = Image.FromFile("assets/texture/front.png");
            this.BackImage = Image.FromFile("assets/texture/back.png");
            this.Image = this.BackImage;
            this.IsFlipped = false;
            this.Click += new EventHandler(CardClick);
        }

        public void FlipOver(Boolean flipp)
        {
            this.Image = flipp ? this.FrontImage : this.BackImage;
            IsFlipped = flipp;
            this.Title = MainForm.FindTextByKeyAndLang(MainForm.LangDataTable, this.File, "Title");
            this.Singer = MainForm.FindTextByKeyAndLang(MainForm.LangDataTable, this.File, "Singer");
        }

        public void FlipOver()
        {
            this.FlipOver(!IsFlipped);
        }

        private void CardClick(object? sender, EventArgs e)
        {
            if (!CardManager.isCanPick() || this.IsFlipped)
            {
                return;
            }
            this.FlipOver();
            CardManager.PickCard(this);
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
                Font font = new Font("微軟正黑體", 12);
                TextFormatFlags flags = TextFormatFlags.WordBreak | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
                SizeF titleSize = TextRenderer.MeasureText(Title, font);
                SizeF authorSize = TextRenderer.MeasureText(Singer, font);
                float x = Padding.Left;
                float y = Padding.Top;
                float titleX = x + (Width + titleSize.Width) / 2;
                float authorX = x + (Width + authorSize.Width) / 2;
                float titleY = y + (Height - Padding.Vertical - titleSize.Height) / 2;
                float authorY = titleY + titleSize.Height + 10;

                TextRenderer.DrawText(e.Graphics, Title, font, new Point((int)titleX, (int)titleY), Color.White, flags);
                TextRenderer.DrawText(e.Graphics, Singer, font, new Point((int)authorX, (int)authorY), Color.White, flags);
            }
        }

    }

}
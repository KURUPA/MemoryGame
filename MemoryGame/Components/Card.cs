namespace MemoryGame
{
    public class Card : PictureBox
    {
        public readonly CardManager CardManager;
        public readonly Image FrontImage;
        public readonly Image BackImage;
        private bool IsFlipped { get; set; }
        public bool isSong { get; set; }

        public String Key { get; set; }
        public String Lang { get; set; }
        public int Id { get; set; }

        private string ShowText { get; set; }
        public static readonly int CARD_WIDTH = 120;
        public static readonly int CARD_HEIGHT = 60;

        public Card(int x, int y, CardManager cardManager, String key, int index) : this(x, y, cardManager)
        {
            this.Key = key;
            this.Id = index;
        }

        public Card(int x, int y, CardManager cardManager) : this(cardManager)
        {
            this.Location = new Point(x, y);
        }

        public Card(CardManager cardManager)
        {
            this.ShowText = "";
            this.Key = "";
            this.Lang = "en_us";
            this.CardManager = cardManager;
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Size = new Size(CARD_WIDTH, CARD_HEIGHT);
            this.FrontImage = Image.FromFile("assets/texture/front.png");
            this.BackImage = Image.FromFile("assets/texture/back.png");
            this.Image = this.BackImage;
            this.BackColor = Color.Aqua;
            this.IsFlipped = false;
            this.Click += new EventHandler(CardClick);
        }

        public void FlipOver(Boolean flipp)
        {
            this.Image = flipp ? this.FrontImage : this.BackImage;
            IsFlipped = flipp;
            this.ShowText = MainForm.FindTextByKeyAndLang(MainForm.LangDataTable, this.Key, this.Lang);
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

            if (IsFlipped && !string.IsNullOrEmpty(ShowText))
            {
                // 設定字型和字體大小
                Font font = new Font("Arial", 12);

                // 計算文字應該放置的位置
                float x = Padding.Left;
                float y = Padding.Top;

                // 計算文字可使用的區域
                RectangleF textArea = new RectangleF(x, y, Width - Padding.Horizontal, Height - Padding.Vertical);

                // 設定文字格式化選項，包括自動換行和對齊方式
                TextFormatFlags flags = TextFormatFlags.WordBreak | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;

                // 在中央繪製文字
                TextRenderer.DrawText(e.Graphics, ShowText, font, Rectangle.Round(textArea), Color.Black, flags);
            }
        }

    }

}
using System.Drawing;

namespace MemoryGame
{
    public class TextPictureBox : PictureBox
    {
        public List<string> TextList;
        private bool ShowText { get; set; }
        private Font font;

        public TextPictureBox(int x, int y, int width, int height, Image image, List<string> textList,Font font) : this()
        {
            this.Size = new Size(width, height);
            this.Location = new Point(x, y);
            this.Image = image;
            this.TextList = textList;
            this.font = font;
        }

        public TextPictureBox()
        {
            this.ShowText = true;
            this.BackColor = Color.Transparent;
            this.TextList = new List<string> { };
            this.font = Tabs.MainMenu.getCubicFont();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (ShowText)
            {
                Brush brush = new SolidBrush(Color.DarkBlue);

                List<SizeF> textSizeList = new List<SizeF>();
                foreach (string text in TextList)
                {
                    SizeF textSize = TextRenderer.MeasureText(text, font);
                    textSizeList.Add(textSize);
                }
                float totalTextHeight = textSizeList.Sum(size => size.Height);
                float spacing = (this.Height - totalTextHeight) / (TextList.Count + 1);
                float currentY = spacing;
                foreach (string text in TextList)
                {
                    SizeF textSize = TextRenderer.MeasureText(text, font);
                    PointF textPoint = new PointF((this.Width - textSize.Width) / 2, currentY);
                    e.Graphics.DrawString(text, font, brush, textPoint);
                    currentY += textSize.Height + spacing;
                }
            }
        }

    }

}
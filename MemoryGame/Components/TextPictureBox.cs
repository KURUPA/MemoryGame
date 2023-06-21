using System.Drawing;

namespace MemoryGame
{
    public class TextPictureBox : PictureBox
    {
        public List<string> TextList;
        private bool ShowText { get; set; }

        public TextPictureBox(int x, int y, int width, int height, Image image, List<string> textList) : this()
        {
            this.Size = new Size(width, height);
            this.Location = new Point(x, y);
            this.Image = image;
            this.TextList = textList;
        }

        public TextPictureBox()
        {
            this.ShowText = true;
            this.BackColor = Color.Transparent;
            this.TextList = new List<string> { };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (ShowText)
            {
                Font font = MemoryGame.Tabs.MainMenu.getCubicFont();
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
namespace MemoryGame.Tabs;

public class Description3 : TabPage
{

    private Button buttonStart;
    public TabControl tabControl;
    private Size formClientSize;
    public Description3(TabControl tabControl, Size formClientSize)
    {
        this.Text = "Description3";
        this.BorderStyle = BorderStyle.None;
        this.tabControl = tabControl;
        this.formClientSize = formClientSize;
        this.BackgroundImage = Image.FromFile("assets/texture/Background.png");

        SuspendLayout();

        TextPictureBox textPictureBox = new TextPictureBox((formClientSize.Width - 450) / 2, 68, 450, 90, Image.FromFile("assets/texture/description.png"), new List<string> { "播放音樂後，根據內容從選項中選取正確的歌名與歌手。" });
        this.Controls.Add(textPictureBox);

        this.buttonStart = new Button();
        this.buttonStart.Font = MainMenu.getCubicFont();
        this.buttonStart.Name = "buttonStart";
        this.buttonStart.Size = new Size(75, 30);
        this.buttonStart.Location = new Point((formClientSize.Width - buttonStart.Width) / 2, 261);
        this.buttonStart.TabIndex = 0;
        this.buttonStart.Text = "開始關卡";
        this.buttonStart.TextAlign = ContentAlignment.MiddleCenter;
        this.buttonStart.UseVisualStyleBackColor = true;
        this.buttonStart.Click += (s, e) => tabControl.SelectedIndex = 6;
        this.Controls.Add(this.buttonStart);

        ResumeLayout();
    }

}
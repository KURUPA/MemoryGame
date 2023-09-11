namespace MemoryGame.Tabs;

public class Description1 : TabPage
{

    private Button buttonStart;
    private TextPictureBox textPictureBox;
    public TabControl tabControl;
    private Size formClientSize;
    public Description1(TabControl tabControl, Size formClientSize)
    {
        this.Text = "Description1";
        this.BorderStyle = BorderStyle.None;
        this.tabControl = tabControl;
        this.formClientSize = formClientSize;
        this.BackgroundImage = Image.FromFile("assets/texture/Background.png");

        SuspendLayout();

        textPictureBox = new TextPictureBox((formClientSize.Width - 450) / 2, 68, 450, 90, Image.FromFile("assets/texture/description.png"), new List<string> { "播放音樂後，根據內容從選項中選取正確的歌名與歌手。" }, MainMenu.getCubicFont());
        this.Controls.Add(textPictureBox);

        this.buttonStart = new Button();
        this.buttonStart.Font = MainMenu.getCubicFont();
        this.buttonStart.Name = "buttonStart";
        this.buttonStart.Size = new Size(100, 30);
        this.buttonStart.Location = new Point((formClientSize.Width - buttonStart.Width) / 2, 261);
        this.buttonStart.TabIndex = 0;
        this.buttonStart.Text = "開始關卡";
        this.buttonStart.TextAlign = ContentAlignment.MiddleCenter;
        this.buttonStart.UseVisualStyleBackColor = true;
        this.buttonStart.Click += (s, e) => tabControl.SelectedIndex = 2;
        this.Controls.Add(this.buttonStart);

        ResumeLayout();
    }

    
    public void init()
    {
        MainForm.InitControlPos(textPictureBox, tabControl.Size, 0.5, 0.3);
        MainForm.InitControlPos(buttonStart, tabControl.Size, 0.5, 0.5);
    }

}
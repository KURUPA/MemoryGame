namespace MemoryGame.Tabs;

public class Description1 : TabPage
{

    private Button buttonStart;
    private Label Title;
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

        this.Title = new Label
        {
            Width = 800,
            Height = 120,
            Location = new Point((tabControl.Width - Width) / 2, Height),
            Text = "播放音樂後，根據內容從選項中選取正確的歌名與歌手。",
            Font = MainMenu.getCubicFont()

        }; this.Controls.Add(Title);

        this.buttonStart = new Button
        {
            Font = MainMenu.getCubicFont(),
            Name = "buttonStart",
            Size = new Size(100, 30),
            Location = new Point((formClientSize.Width - Width) / 2, 261),
            TabIndex = 0,
            Text = "開始關卡",
            TextAlign = ContentAlignment.MiddleCenter,
            UseVisualStyleBackColor = true
        };
        this.buttonStart.Click += (s, e) => tabControl.SelectedIndex = 2;
        this.Controls.Add(this.buttonStart);

        ResumeLayout();
    }


    public void init()
    {
        MainForm.InitControlPos(Title, tabControl.Size, 0.5, 0.3);
        MainForm.InitControlPos(buttonStart, tabControl.Size, 0.5, 0.5);
    }

}
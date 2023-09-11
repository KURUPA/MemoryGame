namespace MemoryGame.Tabs;

using System.Drawing.Text;

public class MainMenu : TabPage
{
    private Button buttonStart1;
    private Button buttonStart2;
    private Button buttonStart3;
    private Button buttonScoreboard;
    public TabControl tabControl;
    private TextPictureBox textPictureBox;
    public MainMenu(TabControl tabControl)
    {
        this.Text = "MainMenu";
        this.BorderStyle = BorderStyle.None;
        this.BackgroundImage = Image.FromFile("assets/texture/Background.png");
        this.tabControl = tabControl;
        SuspendLayout();
        TextPictureBox textPictureBox = new((tabControl.Width - 180) / 2, 68, 360, 60, Image.FromFile("assets/texture/description.png"), new List<string> { "延緩失智遊戲" }, getCubicFont(30));
        this.textPictureBox = textPictureBox;
        this.buttonStart1 = new Button
        {
            Name = "buttonStart1",
            Size = new Size(100, 30),
            TabIndex = 0,
            Text = "第一關",
            TextAlign = ContentAlignment.MiddleCenter,
            UseVisualStyleBackColor = true,
            Font = getCubicFont()
        };
        this.buttonStart2 = new Button
        {
            Name = "buttonStart2",
            Size = new Size(100, 30),
            TabIndex = 0,
            Text = "第二關",
            TextAlign = ContentAlignment.MiddleCenter,
            UseVisualStyleBackColor = true,
            Font = getCubicFont()
        };
        this.buttonStart3 = new Button
        {
            Name = "buttonStart3",
            Size = new Size(100, 30),
            TabIndex = 0,
            Text = "第三關",
            TextAlign = ContentAlignment.MiddleCenter,
            UseVisualStyleBackColor = true,
            Font = getCubicFont()
        };
        this.buttonScoreboard = new Button
        {
            Name = "buttonScoreboard",
            Size = new Size(100, 30),
            TabIndex = 0,
            Text = "記分板",
            TextAlign = ContentAlignment.MiddleCenter,
            UseVisualStyleBackColor = true,
            Font = getCubicFont()
        };

        this.Controls.Add(buttonStart1);
        this.Controls.Add(buttonStart2);
        this.Controls.Add(buttonStart3);
        this.Controls.Add(buttonScoreboard);
        this.Controls.Add(textPictureBox);
        ResumeLayout();
        this.buttonStart1.Click += (sender, args) => tabControl.SelectedIndex = 1;
        this.buttonStart2.Click += (sender, args) => tabControl.SelectedIndex = 3;
        this.buttonStart3.Click += (sender, args) => tabControl.SelectedIndex = 5;
        this.buttonScoreboard.Click += (sender, args) => tabControl.SelectedIndex = 7;
        init();
    }

    public static Font getCubicFont()
    {
        return getCubicFont(12);
    }
    public static Font getCubicFont(int Size)
    {
        PrivateFontCollection fontcollection = new PrivateFontCollection();
        fontcollection.AddFontFile("assets/font/Cubic_11_1.010_R.ttf");
        Font font = new Font(fontcollection.Families[0], Size);
        return font;
    }

    public void init()
    {
        MainForm.InitControlPos(textPictureBox, tabControl.Size, 0.5, 0.1);
        MainForm.InitControlPos(buttonStart1, tabControl.Size, 0.5, 0.3);
        MainForm.InitControlPos(buttonStart2, tabControl.Size, 0.5, 0.4);
        MainForm.InitControlPos(buttonStart3, tabControl.Size, 0.5, 0.5);
        MainForm.InitControlPos(buttonScoreboard, tabControl.Size, 0.5, 0.6);
    }

}
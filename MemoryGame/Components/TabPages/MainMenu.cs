namespace MemoryGame.Tabs;

using System.Drawing.Text;

public class MainMenu : TabPage
{
    private Button buttonStart;
    private SongTitleManager cardManager;
    public TabControl tabControl;
    private MainForm form;
    public MainMenu(TabControl tabControl, MainForm form)
    {
        this.Text = "MainMenu";
        this.BorderStyle = BorderStyle.None;
        this.BackgroundImage = Image.FromFile("assets/texture/Background.png");
        this.tabControl = tabControl;
        this.form = form;
        this.cardManager = new SongTitleManager();
        SuspendLayout();
        Size formClientSize = form.ClientSize;
        TextPictureBox textPictureBox = new TextPictureBox((formClientSize.Width - 180) / 2, 68, 180, 30, Image.FromFile("assets/texture/description.png"), new List<string> { "失智老人猜歌啥的" });
        this.buttonStart = new Button();
        this.buttonStart.Name = "buttonStart";
        this.buttonStart.Size = new Size(75, 30);
        this.buttonStart.Location = new Point((formClientSize.Width - buttonStart.Width) / 2, 261);
        this.buttonStart.TabIndex = 0;
        this.buttonStart.Text = "開始遊戲";
        this.buttonStart.TextAlign = ContentAlignment.MiddleCenter;
        this.buttonStart.UseVisualStyleBackColor = true;
        this.buttonStart.Font = getCubicFont();

        this.Controls.Add(buttonStart);
        this.Controls.Add(textPictureBox);

        Button test = new Button();
        test.MouseClick += (s, e) =>
        {
            this.form.Level1Time = new TimeSpan(0, 0, 59);
            this.form.Level2Time = new TimeSpan(0, 1, 57);
            this.form.Level3Time = new TimeSpan(0, 3, 28);
            this.form.scoreboard.Serialization();
        };
        this.Controls.Add(test);
        ResumeLayout();
        this.buttonStart.Click += (sender, args) => tabControl.SelectedIndex = 1;
    }
    private SongTitle CreateCard(int x, int y, int index, String key)
    {
        return new SongTitle(x, y, this.cardManager, key, index);
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

}
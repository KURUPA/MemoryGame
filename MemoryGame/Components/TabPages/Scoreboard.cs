namespace MemoryGame.Tabs;

public class Scoreboard : TabPage
{
    public MainForm form;
    public TabControl tabControl;
    private int index;
    public Scoreboard(TabControl tabControl, MainForm form)
    {
        this.tabControl = tabControl;
        this.form = form;
        this.index = 0;
        this.init();
        this.Text = "Scoreboard";
        this.BorderStyle = BorderStyle.None;
        this.BackgroundImage = Image.FromFile("assets/texture/background.png");
    }
    private void init()
    {
        PictureBox back = new PictureBox();
        back.Size = new Size(800, 400);
        back.Image = Image.FromFile("assets/texture/scoreboard.png");
        back.Location = new Point(-3, 6);
        Label titleTime = createTitle(36 + 85, "通關日期");
        Label titleLevel1 = createTitle(36 + 85 + 175, "第1關時間");
        Label titleLevel2 = createTitle(36 + 85 + 175 * 2, "第2關時間");
        Label titleLevel3 = createTitle(36 + 85 + 175 * 3, "第3關時間");
        this.Controls.Add(back);
    }

    private Label createTitle(int x, string text)
    {
        Label label = new Label();
        label.Text = text;
        label.Size = new Size(175, 40);
        label.Location = new Point(x + label.Width / 2, 60);
        label.Font = MainMenu.getCubicFont(36);
        this.Controls.Add(label);
        return label;
    }
}
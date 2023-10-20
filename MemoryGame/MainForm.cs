namespace MemoryGame;
using MemoryGame.Tabs;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
        songDataTable = createDataTable();
        tabControl.Size = new Size(1920, 1080);
        tabControl.Location = new Point(0, -80);
        this.menu = new MainMenu(tabControl);
        this.scoreboard = new Scoreboard(tabControl, this);
        InitializeCard();
    }
}

namespace MemoryGame;
using MemoryGame.Tabs;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
        songDataTable = CreateDataTable();
        tabControl.Size = new Size(1920, 1080);
        tabControl.Location = new Point(0, -80);
        this.menu = new MainMenu(tabControl);
        this.MinimumSize = new Size(960, 540);
        InitializeCard();
    }
}

namespace MemoryGame;
using MemoryGame.Tabs;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
        songDataTable = createDataTable();
        tabControl.Size = this.Size;
        this.menu = new MainMenu(tabControl);
        this.description1 = new Description1(tabControl, this.ClientSize);
        this.description2 = new Description2(tabControl, this.ClientSize);
        this.description3 = new Description3(tabControl, this.ClientSize);
        this.level1 = new Level1(tabControl, this);
        this.level2 = new Level2(tabControl, this);
        this.level3 = new Level3(tabControl, this);
        this.scoreboard = new Scoreboard(tabControl, this);
        this.SizeChanged += (s, e) =>
        {
            tabControl.Size = this.Size;
            menu.init();
            menu.init();
            menu.init();
            menu.init();
        };
        InitializeCard();
    }
}

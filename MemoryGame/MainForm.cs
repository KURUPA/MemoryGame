namespace MemoryGame;
using MemoryGame.Tabs;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
        LangDataTable = createLangDataTable();
        this.menu = new MainMenu(tabControl, this.ClientSize);
        this.description1 = new Description1(tabControl, this.ClientSize);
        this.level1 = new Level1(tabControl, this.ClientSize);
        InitializeCard();
    }
}

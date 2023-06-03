namespace MemoryGame
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            InitializeCard();
            cardManager = new CardManager();
        }

    }
}
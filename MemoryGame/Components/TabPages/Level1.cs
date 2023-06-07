namespace MemoryGame.Tabs;

public class Level1 : TabPage
{
    private CardManager cardManager;
    public TabControl tabControl;
    private Size formClientSize;
    private Button button;
    public Level1(TabControl tabControl, Size formClientSize)
    {
        this.Text = "MainMenu";
        this.tabControl = tabControl;
        this.formClientSize = formClientSize;
        this.cardManager = generateCard();
        this.button = generateButtonPlay();
    }

    private Button generateButtonPlay()
    {
        button = new Button();
        button.Size = new Size(60, 60);
        button.BackgroundImage = Image.FromFile("assets/texture/Play/A_Play1.png");
        return button;

    }

    private CardManager generateCard()
    {
        this.cardManager = new CardManager();
        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                SongTitle card = CreateCard(cardManager, 20 + col * (6 + SongTitle.CARD_WIDTH), 20 + row * (6 + SongTitle.CARD_HEIGHT), row * 10 + col, "cat");
                cardManager.AddCard(card);
            }
        }
        cardManager.RandomlyAssignKeys();
        tabControl.SelectedIndex = 2;
        cardManager.CardList.ForEach(card => this.Controls.Add(card));
        return this.cardManager;


    }

    private SongTitle CreateCard(CardManager cardManager, int x, int y, int index, String key)
    {
        return new SongTitle(x, y, cardManager, key, index);
    }

}
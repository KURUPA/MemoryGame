namespace MemoryGame.Tabs;

using System.Drawing.Text;

public class MainMenu : TabPage
{
    private Button buttonStart;
    private Label labelTitle;
    private CardManager cardManager;
    public TabControl tabControl;
    private Size formClientSize;
    public MainMenu(TabControl tabControl, Size formClientSize)
    {
        this.Text = "MainMenu";
        this.tabControl = tabControl;
        this.formClientSize = formClientSize;
        this.cardManager = new CardManager();
        SuspendLayout();

        Font font = getFont();

        this.labelTitle = new Label();
        this.labelTitle.Name = "labelTitle";
        this.labelTitle.Size = new Size(42, 30);
        this.labelTitle.Location = new Point((formClientSize.Width - labelTitle.Width) / 2, 68);
        this.labelTitle.TabIndex = 1;
        this.labelTitle.Text = "Title";
        this.labelTitle.Font = font;

        this.buttonStart = new Button();
        this.buttonStart.Name = "buttonStart";
        this.buttonStart.Size = new Size(75, 30);
        this.buttonStart.Location = new Point((formClientSize.Width - buttonStart.Width) / 2, 261);
        this.buttonStart.TabIndex = 0;
        this.buttonStart.Text = "START";
        this.buttonStart.UseVisualStyleBackColor = true;
        this.buttonStart.Font = font;

        this.Controls.Add(buttonStart);
        this.Controls.Add(labelTitle);

        ResumeLayout();
        this.buttonStart.Click += (sender, args) => tabControl.SelectedIndex = 1;
    }
    private SongTitle CreateCard(int x, int y, int index, String key)
    {
        return new SongTitle(x, y, this.cardManager, key, index);
    }

    public static Font getFont()
    {
        PrivateFontCollection fontcollection = new PrivateFontCollection();
        fontcollection.AddFontFile("assets/font/Cubic_11_1.010_R.ttf");
        Font font = new Font(fontcollection.Families[0], 12);
        return font;
    }

}
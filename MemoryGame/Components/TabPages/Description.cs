namespace MemoryGame.Tabs;

public class Description : TabPage
{

    private Button buttonStart;
    public TabControl tabControl;
    public Description(TabControl tabControl, string value, int index)
    {
        this.Text = "Description1";
        this.BorderStyle = BorderStyle.None;
        this.tabControl = tabControl;
        this.BackgroundImage = Image.FromFile("assets/texture/Background.png");

        SuspendLayout();

        var Title = new Label
        {
            Width = 800,
            Height = 120,
            Location = new Point((tabControl.Width - 800) / 2, Height),
            Text = value

        }; this.Controls.Add(Title);
        this.buttonStart = new Button
        {
            Font = MainMenu.getCubicFont(),
            Name = "buttonStart",
            Size = new Size(300, 90),
            Location = new Point((tabControl.Width - 300) / 2, 261),
            TabIndex = 0,
            Text = "開始關卡",
            TextAlign = ContentAlignment.MiddleCenter,
            UseVisualStyleBackColor = true
        };
        this.buttonStart.Click += (s, e) => tabControl.SelectedIndex = index;
        this.Controls.Add(this.buttonStart);

        ResumeLayout();
    }

}
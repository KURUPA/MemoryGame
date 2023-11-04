namespace MemoryGame.Tabs;

using System.Drawing.Text;

public class MainMenu : TabPage
{
    private Button buttonStart1;
    public Label timeboard1;
    private Button buttonStart2;
    private Button buttonStart3;
    private Button buttonScoreboard;
    public TabControl tabControl;
    private Label Title;
    public MainMenu(TabControl tabControl)
    {
        this.Text = "MainMenu";
        this.BorderStyle = BorderStyle.None;
        this.tabControl = tabControl;
        SuspendLayout();
        this.Title = new Label
        {
            Size = new Size(480, 120),
            Location = new Point((tabControl.Width - 480) / 2, (int)(tabControl.Height * 0.2)),
            Text = "延緩失智遊戲",
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = Color.White,
        };

        //開始遊戲按鈕
        this.buttonStart1 = new Button
        {
            Name = "buttonStart1",
            Size = new Size(300, 90),
            TabIndex = 0,
            Text = "開始遊戲",
            TextAlign = ContentAlignment.MiddleCenter,
            UseVisualStyleBackColor = true,
            Location = new Point((tabControl.Width - 300) / 2, (int)(tabControl.Height * 0.5))
        };

        this.buttonStart1.Click += (sender, args) => tabControl.SelectedIndex = 1;

        this.timeboard1 = new Label
        {
            Location = new Point((tabControl.Width - 300) / 2 + 300, (int)(tabControl.Height * 0.5) + 25),
            ForeColor = Color.White,
            Size = new Size(560, 90),
            Visible = true,
            Text = ""
        };
        this.Controls.Add(timeboard1);
        this.buttonStart2 = new Button
        {
            Name = "buttonStart2",
            Size = new Size(300, 90),
            TabIndex = 0,
            Text = "第二關",
            TextAlign = ContentAlignment.MiddleCenter,
            UseVisualStyleBackColor = true,
            Location = new Point((tabControl.Width - 300) / 2, (int)(tabControl.Height * 0.6))
        };
        this.buttonStart3 = new Button
        {
            Name = "buttonStart3",
            Size = new Size(300, 90),
            TabIndex = 0,
            Text = "第三關",
            TextAlign = ContentAlignment.MiddleCenter,
            Location = new Point((tabControl.Width - 300) / 2, (int)(tabControl.Height * 0.7)),
            UseVisualStyleBackColor = true
        };
        this.buttonScoreboard = new Button
        {
            Name = "buttonScoreboard",
            Size = new Size(300, 90),
            TabIndex = 0,
            Text = "記分板",
            TextAlign = ContentAlignment.MiddleCenter,
            Location = new Point((tabControl.Width - 300) / 2, (int)(tabControl.Height * 0.8)),
            UseVisualStyleBackColor = true
        };
        this.Controls.Add(Title);
        this.Controls.Add(buttonStart1);
        //this.Controls.Add(buttonStart2);
        //this.Controls.Add(buttonStart3);
        //this.Controls.Add(buttonScoreboard);
        ResumeLayout();
        this.buttonStart2.Click += (sender, args) => tabControl.SelectedIndex = 3;
        this.buttonStart3.Click += (sender, args) => tabControl.SelectedIndex = 5;
        this.buttonScoreboard.Click += (sender, args) => tabControl.SelectedIndex = 7;
    }

    public static Font getCubicFont()
    {
        return getCubicFont(36);
    }
    public static Font getCubicFont(int Size)
    {
        /*
        PrivateFontCollection fontcollection = new PrivateFontCollection();
        fontcollection.AddFontFile("assets/font/Cubic_11_1.010_R.ttf");
        Font font = new Font(fontcollection.Families[0], Size);
        */
        Font font = new System.Drawing.Font("Microsoft JhengHei UI", Size, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        return font;
    }

}
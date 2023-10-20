namespace MemoryGame.Tabs;

using NAudio.Wave;
using System.Diagnostics;
using System.Windows.Forms;
public class Level2 : TabPage, Managerlistener
{
    private readonly Random random;
    private SongTitleManager manager;
    public TabControl tabControl;
    private readonly MainForm form;
    private TimeSpan time;
    private int score;
    private Label timeboard;
    private PictureBox buttonPlay;
    private PictureBox buttonRestart;
    private PictureBox buttonNext;
    private Stopwatch stopwatch;
    private Timer timer;
    public Level2(TabControl tabControl, MainForm form)
    {
        this.tabControl = tabControl;
        this.form = form;
        this.random = new Random();

        this.manager = GenerateCard();
        this.manager.managerlistener = this;
        this.Text = "Level 2";
        this.BorderStyle = BorderStyle.None;
        this.BackgroundImage = Image.FromFile("assets/texture/Background.png");
        this.SetScore(0);
        this.timeboard = new Label
        {
            Font = MainMenu.getCubicFont(64),
            Location = new Point(1300, 800),
            ForeColor = Color.White,
            Size = new Size(560, 110),
            Visible = true,
            Text = "時間：00:00"
        };
        this.stopwatch = new Stopwatch();
        this.timer = new Timer();
        this.timer.Tick += (s, e) => SetTime(stopwatch.Elapsed);
        this.buttonRestart = GenerateButton(-200, "Restart", "重播");
        this.buttonRestart.MouseUp += (s, e) => Play();
        this.buttonNext = GenerateButton(200, "Next", "下一首");
        this.buttonNext.MouseUp += (s, e) => { Next(); };
        this.buttonRestart.Enabled = false;
        this.buttonRestart.Visible = false;
        this.buttonNext.Enabled = false;
        this.buttonNext.Visible = false;
        this.buttonPlay = GenerateButton(0, "Play", "開始");
        this.buttonPlay.MouseDown += (s, e) =>
        {
            Next();
            this.buttonRestart.Enabled = true;
            this.buttonRestart.Visible = true;
            this.buttonNext.Enabled = true;
            this.buttonNext.Visible = true;
            if (buttonPlay != null)
            {
                this.buttonPlay.Enabled = false;
                this.buttonPlay.Visible = false;
            }
            this.manager.CanPick = true;
            this.manager.list.ForEach((song) => song.Visible = true);
            this.timer.Start();
            this.stopwatch.Start();
        };
        this.Controls.Add(this.timeboard);
    }

    private PictureBox GenerateButton(int x, string name, string text)
    {
        PictureBox button = new PictureBox()
        {
            Size = new Size(160, 160),
            Location = new Point((form.Width - 160) / 2 + x, 720),
            SizeMode = PictureBoxSizeMode.Zoom,
            Image = Image.FromFile("assets/texture/" + name + "/A_" + name + "2.png"),
            BackColor = Color.Transparent
        };
        this.Controls.Add(button);
        Label label = new Label()
        {
            Text = text,
            Size = new Size(220, 160),
            Location = new Point((form.Width - 220) / 2 + x, button.Location.Y + 120),
            TextAlign = ContentAlignment.MiddleCenter

        };
        this.Controls.Add(label);
        button.MouseDown += (s, e) =>
                {
                    if (s is PictureBox b)
                    {
                        b.Image = Image.FromFile("assets/texture/" + name + "/A_" + name + "3.png");
                    }
                };
        button.MouseUp += (s, e) =>
        {
            if (s is PictureBox b)
            {
                b.Image = Image.FromFile("assets/texture/" + name + "/A_" + name + "2.png");
            }
        };
        button.VisibleChanged += (s, e) => { label.Visible = button.Visible; label.Enabled = button.Enabled; };

        return button;
    }

    private void Play()
    {
        string song = this.manager.list[random.Next(this.manager.list.Count())].File;
        this.manager.setSong(song);
        var reader = new Mp3FileReader("assets/song/" + song + ".mp3");
        var waveOut = new WaveOut();
        waveOut.Init(reader);
        waveOut.Play();
        Console.WriteLine("Song={0}", song);
    }

    private void Next()
    {
        try
        {
            Play();
        }
        catch (System.Exception)
        {
            Console.WriteLine("No song");
        }
    }

    private SongTitleManager GenerateCard()
    {
        int xxx = (tabControl.Width - (5 * (6 + SongTitle.CARD_WIDTH))) / 2;
        SongTitleManager manager = new SongTitleManager();
        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                SongTitle card = SongTitle.CreateSongTitle(manager, xxx, 60, col, row, row * 10 + col, "", false, false);
                Controls.Add(card);
                manager.AddSongTitle(card);
            }
        }
        manager.RandomlyAssignKeys();
        return manager;
    }
    private void AddScore(int score)
    {
        this.SetScore(this.score + score);
    }

    private void SetScore(int score)
    {
        this.score = score;
    }
    private void SetTime(TimeSpan time)
    {
        this.time = time;
        TimeSpan elapsedTime = this.stopwatch.Elapsed;
        this.timeboard.Text = $"時間：{elapsedTime.Minutes:D2}:{elapsedTime.Seconds:D2}";
        this.timeboard.Size = TextRenderer.MeasureText(this.timeboard.Text, this.timeboard.Font);
    }

    void Managerlistener.CardPick(SongTitle songTitle, bool match)
    {
        if (match)
        {
            CardPickMatch(songTitle);
        }
    }

    void CardPickMatch(SongTitle songTitle)
    {
        songTitle.Visible = false;
        Next();
        AddScore(10);
        if (score >= 200)
        {
            this.form.Level1Time = this.time;
            tabControl.SelectedIndex = 5;
            this.Reset();
        }
    }

    public void Reset()
    {
        this.Controls.Clear();

        this.manager = GenerateCard();
        this.manager.managerlistener = this;
        this.Text = "Level 2";
        this.BorderStyle = BorderStyle.None;
        this.BackgroundImage = Image.FromFile("assets/texture/Background.png");
        this.SetScore(0);
        this.timeboard = new Label
        {
            Font = MainMenu.getCubicFont(64),
            Location = new Point(1300, 800),
            ForeColor = Color.White,
            Size = new Size(560, 110),
            Visible = true,
            Text = "時間：00:00"
        };
        this.stopwatch = new Stopwatch();
        this.timer = new Timer();
        this.timer.Tick += (s, e) => SetTime(stopwatch.Elapsed);
        this.buttonRestart = GenerateButton(-200, "Restart", "重播");
        this.buttonRestart.MouseUp += (s, e) => Play();
        this.buttonNext = GenerateButton(200, "Next", "下一首");
        this.buttonNext.MouseUp += (s, e) => { Next(); };
        this.buttonRestart.Enabled = false;
        this.buttonRestart.Visible = false;
        this.buttonNext.Enabled = false;
        this.buttonNext.Visible = false;
        this.buttonPlay = GenerateButton(0, "Play", "開始");
        this.buttonPlay.MouseDown += (s, e) =>
        {
            Next();
            this.buttonRestart.Enabled = true;
            this.buttonRestart.Visible = true;
            this.buttonNext.Enabled = true;
            this.buttonNext.Visible = true;
            if (buttonPlay != null)
            {
                this.buttonPlay.Enabled = false;
                this.buttonPlay.Visible = false;
            }
            this.manager.CanPick = true;
            this.manager.list.ForEach((song) => song.Visible = true);
            this.timer.Start();
            this.stopwatch.Start();
        };
        this.Controls.Add(this.timeboard);
    }
}
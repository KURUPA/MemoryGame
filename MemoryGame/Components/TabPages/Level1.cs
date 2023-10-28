namespace MemoryGame.Tabs;

using NAudio.Wave;
using System.Diagnostics;
using System.Windows.Forms;
public class Level1 : TabPage, Managerlistener
{
    private readonly Random random;
    private SongTitleManager manager;
    public TabControl tabControl;
    private readonly MainMenu form;
    private TimeSpan time;
    private Label timeboard;
    private PictureBox buttonPlay;
    private PictureBox buttonRestart;
    private PictureBox buttonNext;
    private Stopwatch stopwatch;
    private Timer timer;
    private WaveOut waveOut;
    public Level1(TabControl tabControl, MainMenu form)
    {
        this.tabControl = tabControl;
        this.form = form;
        this.random = new Random();
        waveOut = new WaveOut();
        this.manager = GenerateCard();
        this.manager.managerlistener = this;
        this.Text = "Level 1";
        this.timeboard = new Label
        {
            Font = MainMenu.getCubicFont(64),
            Location = new Point(1300, 800),
            ForeColor = Color.White,
            Size = new Size(560, 110),
            Visible = true,
            Text = "時間：00:00"
        };
        this.Controls.Add(this.timeboard);
        this.stopwatch = new Stopwatch();
        this.timer = new Timer();
        this.timer.Tick += (s, e) => SetTime(stopwatch.Elapsed);
        this.buttonRestart = GenerateButton(0, "Restart", "重播");
        this.buttonRestart.MouseUp += (s, e) => Play();
        this.buttonNext = GenerateButton(300, "Next", "下一首");
        this.buttonNext.MouseUp += (s, e) =>
        {
            Next();
        };
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
    }

    private PictureBox GenerateButton(int x, string name, string text)
    {
        PictureBox button = new PictureBox()
        {
            Size = new Size(160, 160),
            Location = new Point(80 + x, 720),
            SizeMode = PictureBoxSizeMode.Zoom,
            Image = Image.FromFile("assets/texture/" + name + "/A_" + name + "2.png"),
            BackColor = Color.Transparent
        };
        this.Controls.Add(button);
        Label label = new Label()
        {
            ForeColor = Color.White,
            Text = text,
            Size = new Size(220, 160),
            Location = new Point(50 + x, button.Location.Y + 120),
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
        if (manager.list.Count <= 0)
        {
            return;
        }
        string song = this.manager.list[random.Next(this.manager.list.Count())].File;
        this.manager.setSong(song);
        var reader = new Mp3FileReader("assets/song/" + song + ".mp3");
        waveOut.Init(reader);
        waveOut.Play();
        Console.WriteLine("Song={0}", song);
    }

    private void Next()
    {
        try
        {
            waveOut.Stop();
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
                SongTitle card = SongTitle.CreateSongTitle(manager, xxx, 60, col, row, row * 10 + col, "", false, true);
                Controls.Add(card);
                manager.AddSongTitle(card);
            }
        }
        manager.RandomlyAssignKeys();
        return manager;
    }
    private void SetTime(TimeSpan time)
    {
        this.time = time;
        TimeSpan elapsedTime = this.stopwatch.Elapsed;
        this.timeboard.Text = $"時間：{elapsedTime.Minutes:D2}:{elapsedTime.Seconds:D2}";
    }

    void Managerlistener.CardPick(SongTitle songTitle, bool match)
    {
        if (match)
        {
            CardPickMatch(songTitle);
        }
        else
        {
            //TO DO
            Stopwatch ErrorStopwatch = new Stopwatch();
            ErrorStopwatch.Start();
            var ErrorTimer = new Timer();
            ErrorTimer.Tick += (s, e) => SetTime(stopwatch.Elapsed);

            /*
            Label errorText = new Label()
            {
                ForeColor = Color.Red,
                Text = "錯誤!",
                Location = songTitle.Location,
                Size = new Size(SongTitle.CARD_HEIGHT, SongTitle.CARD_WIDTH)
            };
            errorText.BringToFront();
            this.Controls.Add(errorText);
            */
        }
    }

    void CardPickMatch(SongTitle songTitle)
    {
        try
        {
            songTitle.Visible = false;
            if (manager.list.Count <= 0)
            {
                string timeText = $"通關時間：{this.time.Minutes:D2}:{this.time.Seconds:D2}"; ;
                this.form.timeboard1.Text = timeText;
                Reset();
                tabControl.SelectedIndex = 0;

                return;
            }
            Next();

        }
        catch (System.Exception)
        {
            Console.WriteLine("WTF Boom");

            throw;
        }
    }

    public void Reset()
    {
        this.Controls.Clear();
        this.manager = GenerateCard();
        this.manager.managerlistener = this;
        this.Text = "Level 1";
        this.timeboard = new Label
        {
            Font = MainMenu.getCubicFont(64),
            Location = new Point(1300, 800),
            ForeColor = Color.White,
            Size = new Size(560, 110),
            Visible = true,
            Text = "時間：00:00"
        };
        this.Controls.Add(this.timeboard);
        this.stopwatch = new Stopwatch();
        this.timer = new Timer();
        this.timer.Tick += (s, e) => SetTime(stopwatch.Elapsed);
        this.buttonRestart = GenerateButton(200, "Restart", "重播");
        this.buttonRestart.MouseUp += (s, e) => Play();
        this.buttonNext = GenerateButton(400, "Next", "下一首");
        this.buttonNext.MouseUp += (s, e) =>
        {
            Next();
        };
        this.buttonRestart.Enabled = false;
        this.buttonRestart.Visible = false;
        this.buttonNext.Enabled = false;
        this.buttonNext.Visible = false;
        this.buttonPlay = GenerateButton(200, "Play", "開始");
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
    }
}
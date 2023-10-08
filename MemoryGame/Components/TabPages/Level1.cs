namespace MemoryGame.Tabs;

using NAudio.Wave;
using System.Diagnostics;
using System.Windows.Forms;
public class Level1 : TabPage, Managerlistener
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
    private Mp3FileReader? mp3FileReader;
    private Stopwatch stopwatch;
    private Timer timer;
    public Level1(TabControl tabControl, MainForm form)
    {
        this.tabControl = tabControl;
        this.form = form;
        this.random = new Random();

        this.manager = generateCard();
        this.manager.managerlistener = this;
        this.Text = "Level 1";
        this.BorderStyle = BorderStyle.None;
        this.BackgroundImage = Image.FromFile("assets/texture/Background.png");
        this.setScore(0);
        this.timeboard = new Label
        {
            Location = new Point(20, 340),
            Font = MainMenu.getCubicFont(36)
        };
        this.timeboard.Size = TextRenderer.MeasureText(timeboard.Text, timeboard.Font);
        this.timeboard.ForeColor = Color.White;
        this.stopwatch = new Stopwatch();
        this.timer = new Timer();
        this.timer.Tick += (s, e) => setTime(stopwatch.Elapsed);
        this.buttonRestart = generateButton(-40, "Restart");
        this.buttonRestart.MouseUp += (s, e) => Play();
        this.buttonNext = generateButton(40, "Next");
        this.buttonNext.MouseUp += (s, e) => { Next(); };
        this.buttonRestart.Enabled = false;
        this.buttonRestart.Visible = false;
        this.buttonNext.Enabled = false;
        this.buttonNext.Visible = false;
        this.buttonPlay = generateButton(0, "Play");
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
        this.Controls.Add(this.buttonPlay);
        this.Controls.Add(this.buttonRestart);
        this.Controls.Add(this.buttonNext);


        PictureBox buttonTest = generateButton(160, "Restart");
        buttonTest.MouseUp += (s, e) =>
        {
            manager.list.ForEach(card => this.Controls.Remove(card));
            setScore(200);
        };
        this.Controls.Add(buttonTest);
    }

    private PictureBox generateButton(int x, String name)
    {
        PictureBox button = new PictureBox();
        button.Size = new Size(60, 60);
        button.Location = new Point((form.Width - button.Size.Width) / 2 + x, 340);
        button.Image = Image.FromFile("assets/texture/" + name + "/A_" + name + "2.png");
        button.BackColor = Color.Transparent;
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
        return button;
    }

    private void Play()
    {
        string song = this.manager.list[random.Next(this.manager.list.Count())].File;
        this.manager.setSong(song);
        this.mp3FileReader = new Mp3FileReader("assets/song/" + song + ".mp3");
        using (var waveOut = new WaveOutEvent())
        {
            waveOut.Init(this.mp3FileReader);
            waveOut.Play();
        }
    }

    private void Next()
    {
        try
        {
            string song = this.manager.list[random.Next(this.manager.list.Count())].File;
            this.manager.setSong(song);
            this.mp3FileReader = new Mp3FileReader("assets/song/" + song + ".mp3");
            using (var waveOut = new WaveOutEvent())
            {
                waveOut.Init(this.mp3FileReader);
                waveOut.Play();
            }
            Console.WriteLine("song = {0}", song);

        }
        catch (System.Exception)
        {
            Console.WriteLine("No song");
        }
    }

    private SongTitleManager generateCard()
    {
        SongTitleManager manager = new SongTitleManager();
        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                SongTitle card = SongTitle.CreateSongTitle(manager, col, row, row * 10 + col, "", false, true);
                Controls.Add(card);
                manager.AddSongTitle(card);
            }
        }
        manager.RandomlyAssignKeys();
        return manager;
    }
    private void addScore(int score)
    {
        this.setScore(this.score + score);
    }

    private void setScore(int score)
    {
        this.score = score;
    }
    private void setTime(TimeSpan time)
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
            songTitle.Visible = false;
            addScore(10);

            if (score >= 200)
            {
                this.form.Level1Time = this.time;
                tabControl.SelectedIndex = 3;
                this.reset();
            }
        }
    }

    public void reset()
    {
        this.Controls.Clear();
        this.manager = generateCard();
        this.manager.managerlistener = this;
        this.Text = "Level 1";
        this.BorderStyle = BorderStyle.None;
        this.BackgroundImage = Image.FromFile("assets/texture/Background.png");
        this.setScore(0);
        this.timeboard = new Label();
        this.timeboard.Location = new Point(20, 340);
        this.timeboard.Font = MainMenu.getCubicFont(36);
        this.timeboard.Size = TextRenderer.MeasureText(timeboard.Text, timeboard.Font);
        this.timeboard.ForeColor = Color.White;
        this.stopwatch = new Stopwatch();
        this.timer = new Timer();
        this.timer.Tick += (s, e) => setTime(stopwatch.Elapsed);
        this.buttonRestart = generateButton(-40, "Restart");
        this.buttonRestart.MouseUp += (s, e) => Play();
        this.buttonNext = generateButton(40, "Next");
        this.buttonNext.MouseUp += (s, e) => { Next(); };
        this.buttonRestart.Enabled = false;
        this.buttonRestart.Visible = false;
        this.buttonNext.Enabled = false;
        this.buttonNext.Visible = false;
        this.buttonPlay = generateButton(0, "Play");
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
        this.Controls.Add(this.buttonPlay);
        this.Controls.Add(this.buttonRestart);
        this.Controls.Add(this.buttonNext);
    }
    public void init()
    {
        MainForm.InitControlPos(timeboard, tabControl.Size, 0.5, 0.1);
        MainForm.InitControlPos(buttonPlay, tabControl.Size, 0.5, 0.3);
        MainForm.InitControlPos(buttonRestart, tabControl.Size, 0.5, 0.4);
        MainForm.InitControlPos(buttonNext, tabControl.Size, 0.5, 0.5);
    }
}
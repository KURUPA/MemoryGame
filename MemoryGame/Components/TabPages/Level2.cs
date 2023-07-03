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
    private PictureBox buttonNext;
    private Mp3FileReader? mp3FileReader;
    private Stopwatch stopwatch;
    private Timer timer;
    public Level2(TabControl tabControl, MainForm form)
    {
        this.tabControl = tabControl;
        this.form = form;
        this.manager = generateCard();
        this.manager.managerlistener = this;
        this.manager.CanPick = false;
        this.random = new Random();
        this.Text = "Level 1";
        this.BorderStyle = BorderStyle.None;
        this.BackgroundImage = Image.FromFile("assets/texture/Background.png");
        this.setScore(0);
        this.timeboard = new Label();
        this.timeboard.Location = new Point(20, 340);
        this.timeboard.Font = MainMenu.getCubicFont(36);
        this.timeboard.Text = "時間：00:00";
        this.timeboard.Size = TextRenderer.MeasureText(timeboard.Text, timeboard.Font);
        this.timeboard.ForeColor = Color.White;
        this.stopwatch = new Stopwatch();
        this.timer = new Timer();
        this.timer.Interval = 1000;
        this.timer.Tick += (s, e) => setTime(stopwatch.Elapsed);

        this.buttonNext = generateButton(0, "Next");
        this.buttonNext.MouseUp += (s, e) => { Next(); };
        this.buttonNext.Enabled = false;
        this.buttonNext.Visible = false;

        this.buttonPlay = generateButton(0, "Play");
        this.buttonPlay.MouseDown += (s, e) =>
        {
            Next();
            if (buttonPlay != null)
            {
                this.buttonPlay.Enabled = false;
                this.buttonPlay.Visible = false;
            }
            this.manager.CanPick = true;
            this.buttonNext.Enabled = true;
            this.buttonNext.Visible = true;
            this.timer.Start();
            this.stopwatch.Start();
        };
        this.Controls.Add(this.timeboard);
        this.Controls.Add(this.buttonPlay);
        this.Controls.Add(this.buttonNext);
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
        using (var waveOut = new WaveOutEvent())
        {
            waveOut.Init(this.mp3FileReader);
            waveOut.Play();
        }
    }

    private void Next()
    {
        string song = this.manager.list[random.Next(this.manager.list.Count())].File;
        this.manager.setSong(song);
        this.mp3FileReader = new Mp3FileReader("assets/song/" + song + ".mp3");
        Console.WriteLine("song = {0}", song);
    }

    private SongTitleManager generateCard()
    {
        SongTitleManager manager = new SongTitleManager();
        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                SongTitle card = CreateSongTitle(manager, 20 + col * (6 + SongTitle.CARD_WIDTH), 20 + row * (6 + SongTitle.CARD_HEIGHT), row * 10 + col, "cat");
                manager.AddSongTitle(card);
            }
        }
        manager.RandomlyAssignKeys();
        return manager;
    }

    private SongTitle CreateSongTitle(SongTitleManager cardManager, int x, int y, int index, String key)
    {
        SongTitle songTitle = new SongTitle(x, y, cardManager, key, index);
        this.Controls.Add(songTitle);
        songTitle.setIsShowText(false);
        songTitle.FlipOver(false);
        return songTitle;
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
            addScore(10);
            if (score >= 200)
            {
                this.timer.Stop();
                this.stopwatch.Stop();
                this.form.Level2Time = this.time;
                this.reset();
                this.tabControl.SelectedIndex = 5;
            }
        }
    }

    public void reset()
    {
        this.manager.CanPick = false;
        this.timeboard.Text = "時間：00:00";
        this.timer.Interval = 1000;
        this.buttonPlay.Enabled = true;
        this.buttonPlay.Visible = true;
        this.buttonNext.Enabled = false;
        this.buttonNext.Visible = false;
        this.timer.Stop();
        this.stopwatch.Stop();
        this.stopwatch.Reset();
        this.setScore(0);
        this.manager.list = new List<SongTitle>();
        this.manager = generateCard();
        this.manager.managerlistener = this;
        this.manager.CanPick = false;
    }
}
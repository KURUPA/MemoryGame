namespace MemoryGame.Tabs;

using NAudio.Wave;
using System.Diagnostics;
using System.Windows.Forms;
public class Level3 : TabPage, Managerlistener
{
    private readonly Random random;
    private SongTitleManager manager;
    public TabControl tabControl;
    private readonly MainForm form;
    private TimeSpan time;
    private int score;
    private Label timeboard;
    private Label scoreboard;
    private PictureBox buttonPlay;
    private PictureBox buttonRestart;
    private PictureBox buttonNext;
    private Mp3FileReader? mp3FileReader;
    private Stopwatch stopwatch;
    private Timer timer;
    public Level3(TabControl tabControl, MainForm form)
    {
        this.manager = generateCard();
        this.manager.managerlistener = this;
        this.manager.CanPick = false;
        this.random = new Random();
        this.Text = "Level 1";
        this.BorderStyle = BorderStyle.None;
        this.BackgroundImage = Image.FromFile("assets/texture/Background.png");
        this.tabControl = tabControl;
        this.form = form;
        this.scoreboard = new Label();
        this.scoreboard.Location = new Point((this.form.Width - scoreboard.Size.Width) / 2 + 160, 340);
        this.scoreboard.Font = MainMenu.getCubicFont(36);
        this.setScore(0);
        this.scoreboard.Size = TextRenderer.MeasureText(scoreboard.Text, scoreboard.Font);
        this.scoreboard.ForeColor = Color.White;
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
        this.buttonRestart = generateButton(-40, "Restart");
        this.buttonRestart.Enabled = false;
        this.buttonRestart.Visible = false;
        this.buttonRestart.MouseUp += (s, e) => Play();
        this.buttonPlay = generateButton(-40, "Play");
        this.buttonPlay.MouseDown += (s, e) =>
        {
            Next();
            this.buttonRestart.Enabled = true;
            this.buttonRestart.Visible = true;
            if (buttonPlay != null)
            {
                this.buttonPlay.Enabled = false;
                this.buttonPlay.Visible = false;
            }
            this.manager.CanPick = true;
            this.timer.Start();
            this.stopwatch.Start();
        };
        this.buttonNext = generateButton(40, "Next");
        this.buttonNext.MouseUp += (s, e) => { Next(); };
        this.Controls.Add(this.scoreboard);
        this.Controls.Add(this.timeboard);
        this.Controls.Add(this.buttonPlay);
        this.Controls.Add(this.buttonRestart);
        this.Controls.Add(this.buttonNext);
    }

    private PictureBox generateButton(int x, String name)
    {
        PictureBox button = new PictureBox();
        button.Size = new Size(60, 60);
        button.Location = new Point((form.Width - button.Size.Width) / 2 + x, 340);
        button.Image = Image.FromFile("assets/texture/" + name + "/A_" + name + "1.png");
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
                b.Image = Image.FromFile("assets/texture/" + name + "/A_" + name + "1.png");
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
        Console.WriteLine("song:{0}", song);
    }

    private SongTitleManager generateCard()
    {
        SongTitleManager manager = new SongTitleManager();
        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                SongTitle card = CreateCard(manager, 20 + col * (6 + SongTitle.CARD_WIDTH), 20 + row * (6 + SongTitle.CARD_HEIGHT), row * 10 + col, "cat");
                manager.AddCard(card);
            }
        }
        manager.RandomlyAssignKeys();
        manager.list.ForEach(card => { this.Controls.Add(card); card.setIsShowText(true); card.FlipOver(false); });
        return manager;
    }

    private SongTitle CreateCard(SongTitleManager cardManager, int x, int y, int index, String key)
    {
        SongTitle songTitle = new SongTitle(x, y, cardManager, key, index);
        return songTitle;
    }
    private void addScore(int score)
    {
        this.setScore(this.score + score);
    }

    private void setScore(int score)
    {
        this.score = score;
        this.scoreboard.Text = "分數：" + score;
        this.scoreboard.Size = TextRenderer.MeasureText(scoreboard.Text, scoreboard.Font);
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
                this.form.level3Time = this.time;
                this.reset();
                this.tabControl.SelectedIndex = 7;
            }
        }
    }
    public void reset()
    {
        this.timer.Stop();
        this.stopwatch.Stop();
        this.setScore(0);
        this.setTime(TimeSpan.Zero);
        this.manager = generateCard();
        this.manager.managerlistener = this;
        this.manager.CanPick = false;
        this.buttonRestart.Enabled = false;
        this.buttonRestart.Visible = false;
    }
}
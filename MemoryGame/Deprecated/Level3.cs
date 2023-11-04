namespace MemoryGame.Tabs;

using NAudio.Wave;
using System.Diagnostics;
using System.Windows.Forms;
using System.Data;
public class Level3 : TabPage, Managerlistener
{
    private List<string> keys;
    private TextBox textBox;
    private PictureBox buttonAccept;
    private string NowSong;
    private readonly Random random;
    public TabControl tabControl;
    private readonly MainMenu form;
    private TimeSpan time;
    private int score;
    private Label timeboard;
    private Label remainingSongs;
    private PictureBox buttonPlay;
    private PictureBox buttonRestart;
    private PictureBox buttonNext;
    private Mp3FileReader? mp3FileReader;
    private Stopwatch stopwatch;
    private Timer timer;
    public Level3(TabControl tabControl, MainMenu form)
    {
        this.tabControl = tabControl;
        this.form = form;
        this.random = new Random();
        this.NowSong = "";
        this.keys = generateFiles();
        this.textBox = new TextBox
        {
            Size = new Size(900, 300),
            Font = MainMenu.getCubicFont(60),
            Location = new Point((form.Width - 900) / 2, (form.Height - 300) / 2 - 100)
        };
        this.buttonAccept = GenerateButton(textBox.Width / 2 + 200, textBox.Location.Y - 40, "Accept", "確定");
        this.buttonAccept.MouseClick += (s, e) => Checking();
        this.Text = "Level 3";
        this.BorderStyle = BorderStyle.None;
        this.BackgroundImage = Image.FromFile("assets/texture/Background.png");
        this.setScore(0);
        this.timeboard = new Label
        {
            Font = MainMenu.getCubicFont(64),
            Location = new Point(1300, 800),
            ForeColor = Color.White,
            Size = new Size(560, 110),
            Visible = true,
            Text = "時間：00:00"
        };
        this.remainingSongs = new Label
        {
            Font = MainMenu.getCubicFont(64),
            Location = new Point(10, 800),
            ForeColor = Color.White,
            Size = new Size(560, 110),
            Visible = true,
            Text = "剩餘：" + keys.Count(),
        };
        this.stopwatch = new Stopwatch();
        this.timer = new Timer
        {
            Interval = 1000
        };
        this.timer.Tick += (s, e) => setTime(stopwatch.Elapsed);
        this.buttonRestart = GenerateButton(-200, "Restart", "重播");
        this.buttonRestart.Enabled = false;
        this.buttonRestart.Visible = false;
        this.buttonRestart.MouseUp += (s, e) => Play();
        this.buttonNext = GenerateButton(200, "Next", "下一首");
        this.buttonNext.MouseUp += (s, e) => { Next(); };
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
            this.timer.Start();
            this.stopwatch.Start();
        };
        this.Controls.Add(this.timeboard);
        this.Controls.Add(this.remainingSongs);
        this.Controls.Add(this.textBox);
    }
    private PictureBox GenerateButton(int x, string name, string text)
    {
        return GenerateButton(x, 720, name, text);
    }

    private PictureBox GenerateButton(int x, int y, string name, string text)
    {
        PictureBox button = new PictureBox()
        {
            Size = new Size(160, 160),
            Location = new Point((form.Width - 160) / 2 + x, y),
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

    private void Checking()
    {
        if (CheckValuesInSameRow(NowSong, textBox.Text))
        {
            keys.Remove(NowSong);
            setScore(score + 10);
            this.textBox.Clear();
            this.remainingSongs.Text = "剩餘：" + keys.Count();
            this.buttonRestart.Enabled = false;
            this.buttonRestart.Image = Image.FromFile("assets/texture/Restart/A_Restart1.png");
        }
    }
    private bool CheckValuesInSameRow(object value1, object value2)
    {
        foreach (DataRow row in MainForm.songDataTable.Rows)
        {
            if (row.ItemArray.Contains(value1) && row.ItemArray.Contains(value2))
            {
                return true;
            }
        }
        return false;
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
        this.buttonRestart.Enabled = true;
        this.buttonRestart.Image = Image.FromFile("assets/texture/Restart/A_Restart2.png");
        string song = this.keys[random.Next(this.keys.Count())];
        this.NowSong = song;
        this.mp3FileReader = new Mp3FileReader("assets/song/" + song + ".mp3");
        Console.WriteLine("song = {0}", song);
    }

    private List<string> generateFiles()
    {
        List<string> Files = new List<string>();
        foreach (DataRow row in MainForm.songDataTable.Rows)
        {
            string? file = row["File"].ToString();
            if (file != null)
            {
                Files.Add(file);
            }
        }
        return Files;
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
                //this.form.Level3Time = this.time;
                this.reset();
                //form.scoreboard.Serialization();
                this.tabControl.SelectedIndex = 7;
            }
        }
    }

    public void reset()
    {
        this.Controls.Clear();
        this.NowSong = "";
        this.keys = generateFiles();
        this.textBox = new TextBox
        {
            Size = new Size(560, 60),
            Font = MainMenu.getCubicFont(36),
            Location = new Point((form.Width - 560) / 2, (form.Height - 60) / 2 - 100)
        };
        this.buttonAccept = GenerateButton(textBox.Width / 2 + 60, textBox.Location.Y, "Accept", "確認");
        this.buttonAccept.MouseClick += (s, e) => Checking();
        this.Text = "Level 3";
        this.BorderStyle = BorderStyle.None;
        this.BackgroundImage = Image.FromFile("assets/texture/Background.png");
        this.setScore(0);
        this.timeboard = new Label
        {
            Font = MainMenu.getCubicFont(64),
            Location = new Point(1300, 800),
            ForeColor = Color.White,
            Size = new Size(560, 110),
            Visible = true,
            Text = "時間：00:00"
        };
        this.remainingSongs = new Label
        {
            Font = MainMenu.getCubicFont(64),
            Location = new Point(10, 800),
            ForeColor = Color.White,
            Size = new Size(560, 110),
            Visible = true,
            Text = "剩餘：" + keys.Count(),
        };
        this.stopwatch = new Stopwatch();
        this.timer = new Timer
        {
            Interval = 1000
        };
        this.timer.Tick += (s, e) => setTime(stopwatch.Elapsed);
        this.buttonRestart = GenerateButton(-40, "Restart", "重播");
        this.buttonRestart.Enabled = false;
        this.buttonRestart.Visible = false;
        this.buttonRestart.MouseUp += (s, e) => Play();
        this.buttonNext = GenerateButton(40, "Next", "下一首");
        this.buttonNext.MouseUp += (s, e) => { Next(); };
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
            this.timer.Start();
            this.stopwatch.Start();
        };
        this.Controls.Add(this.timeboard);
        this.Controls.Add(this.remainingSongs);
        this.Controls.Add(this.buttonPlay);
        this.Controls.Add(this.buttonRestart);
        this.Controls.Add(this.buttonNext);
        this.Controls.Add(this.buttonAccept);
        this.Controls.Add(this.textBox);
    }
}
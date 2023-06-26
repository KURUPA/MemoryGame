namespace MemoryGame.Tabs;

using NAudio.Wave;
using System.Diagnostics;
using System.Windows.Forms;
using System.Data;
public class Level3 : TabPage, Managerlistener
{
    private List<string> keys;
    private TextBox textBox;
    private PictureBox buttonConfirm;
    private string NowSong;
    private readonly Random random;
    public TabControl tabControl;
    private readonly MainForm form;
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
    public Level3(TabControl tabControl, MainForm form)
    {
        this.NowSong = "";
        this.keys = generateFiles();
        this.remainingSongs = new Label();
        this.textBox = new TextBox();
        this.buttonConfirm = generateButton(0, "Restart");
        this.buttonConfirm.MouseClick += (s, e) => Checking();
        this.random = new Random();
        this.Text = "Level 1";
        this.BorderStyle = BorderStyle.None;
        this.BackgroundImage = Image.FromFile("assets/texture/Background.png");
        this.tabControl = tabControl;
        this.form = form;
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
            this.timer.Start();
            this.stopwatch.Start();
        };
        this.buttonNext = generateButton(40, "Next");
        this.buttonNext.MouseUp += (s, e) => { Next(); };
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

    private void Checking()
    {
        if (CheckValuesInSameRow(NowSong, textBox.Text))
        {
            keys.Remove(NowSong);
            setScore(score + 10);
        }
    }
    private bool CheckValuesInSameRow(object value1, object value2)
    {
        foreach (DataRow row in MainForm.dataTable.Rows)
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
    }

    private List<string> generateFiles()
    {
        List<string> Files = new List<string>();
        foreach (DataRow row in MainForm.dataTable.Rows)
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
                this.form.Level3Time = this.time;
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
        this.buttonRestart.Enabled = false;
        this.buttonRestart.Visible = false;
    }
}
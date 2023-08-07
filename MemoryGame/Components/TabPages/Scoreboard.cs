namespace MemoryGame.Tabs;

using System.Data;
using Newtonsoft.Json.Linq;

public class Scoreboard : TabPage
{
    public MainForm form;
    public TabControl tabControl;
    private List<Label> scoreList;
    private int index;
    public Scoreboard(TabControl tabControl, MainForm form)
    {
        this.tabControl = tabControl;
        this.form = form;
        this.index = 0;
        this.scoreList = new List<Label>();
        this.init();
        this.Text = "Scoreboard";
        this.BorderStyle = BorderStyle.None;
        this.BackgroundImage = Image.FromFile("assets/texture/background.png");
        PictureBox buttonLeft = generateButton(10, "Left");
        PictureBox buttonRight = generateButton(10, "Right");
        buttonLeft.MouseUp += (s, e) => { if (this.index > 0) { this.index--; } };
        buttonRight.MouseUp += (s, e) => { if (this.index < 127) { this.index--; } };
    }
    private void init()
    {
        if (this.scoreList.Count > 0)
        {
            scoreList.ForEach(l => this.Controls.Remove(l));
        }
        PictureBox back = new PictureBox();
        back.Size = new Size(800, 400);
        back.Image = Image.FromFile("assets/texture/scoreboard.png");
        back.Location = new Point(-3, 6);
        int xOffset = -40;
        int yOffset = 185;
        createTitle(xOffset, 36, "通關日期", true, 20);
        createTitle(xOffset + yOffset, 36, "第1關時間", true, 20);
        createTitle(xOffset + yOffset * 2, 36, "第2關時間", true, 20);
        createTitle(xOffset + yOffset * 3, 36, "第3關時間", true, 20);
        addScoreboard(this.index);
        this.Controls.Add(back);
    }

    private void addScoreboard(int index)
    {
        DataTable dataTable = Deserialization();
        if (dataTable.Rows.Count == 0)
        {
            return;
        }

        for (int row = index * 4; row < index * 4 + 4; row++)
        {
            if (dataTable.Rows.Count <= row)
            {
                return;
            }
            DataRow dataRow = dataTable.Rows[row];
            if (dataRow != null)
            {
                for (int col = 0; col < 4; col++)
                {
                    string? text = dataRow[col].ToString();
                    Console.WriteLine("text={0}", text);
                    if (text != null)
                    {
                        createTitle(-40 + 185 * col, 90 + 60 * row, text, false, 16);
                    }
                }
            }
        }
    }

    private Label createTitle(int x, int y, string text, bool bold, int size)
    {
        Label label = new Label();
        label.Text = text;
        label.Size = new Size(160, 60);
        label.Location = new Point(x + label.Width / 2, y);
        label.Font = bold ? new Font(MainMenu.getCubicFont(size), FontStyle.Bold) : MainMenu.getCubicFont(size);
        label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        label.BackColor = Color.FromArgb(255, 234, 212, 170);
        this.scoreList.Add(label);
        this.Controls.Add(label);
        return label;
    }

    private static DataTable Deserialization()
    {
        string jsonFilePath = "assets/data/scoreboard.json";
        string jsonData = File.ReadAllText(jsonFilePath);
        JArray json = JArray.Parse(jsonData);
        DataTable dataTable = new DataTable();
        dataTable.Columns.Add("Date");
        dataTable.Columns.Add("Time1");
        dataTable.Columns.Add("Time2");
        dataTable.Columns.Add("Time3");
        foreach (JToken item in json)
        {
            JToken? date = item["Date"];
            JToken? time1 = item["Time1"];
            JToken? time2 = item["Time2"];
            JToken? time3 = item["Time3"];
            if (date != null && time1 != null && time2 != null && time3 != null)
            {
                DataRow row = dataTable.NewRow();
                row["Date"] = date.ToString();
                row["Time1"] = time1.ToString();
                row["Time2"] = time2.ToString();
                row["Time3"] = time3.ToString();
                dataTable.Rows.Add(row);
            }
        }
        return dataTable;
    }

    public void Serialization()
    {
        string jsonFilePath = "assets/data/scoreboard.json";
        JArray json = new JArray();
        JObject item = new JObject();
        item.Add("Date", System.DateTime.Now.ToString());
        item.Add("Time1", this.form.Level1Time);
        item.Add("Time2", this.form.Level2Time);
        item.Add("Time3", this.form.Level3Time);
        json.Add(item);
        string jsonData = json.ToString();
        File.WriteAllText(jsonFilePath, jsonData);
    }

    private PictureBox generateButton(int x, String name)
    {
        PictureBox button = new PictureBox();
        button.Size = new Size(60, 60);
        button.Location = new Point((form.Width - button.Size.Width) / 2 + x, 360);
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
        this.Controls.Add(button);
        return button;
    }
}
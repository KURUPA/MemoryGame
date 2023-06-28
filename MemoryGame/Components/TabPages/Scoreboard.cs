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
    }
    private void init()
    {
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
        int xOffset = -40;
        int yOffset = 185;
        DataTable dataTable = Deserialization();
        if (dataTable.Rows.Count == 0)
        {
            return;
        }
        for (int row = index * 5; row < index * 5 + 5; row++)
        {
            DataRow dataRow = dataTable.Rows[row];
            if (dataRow != null)
            {
                for (int col = 0; col < 4; col++)
                {
                    string? text = dataRow[col].ToString();
                    if (text != null)
                    {
                        createTitle(xOffset + yOffset * col, 36, text, false, 16);
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
            if (item["Date"] != null && item["Time1"] != null && item["Time2"] != null && item["Time3"] != null)
            {
                DataRow row = dataTable.NewRow();
                row["Date"] = item["Date"]?.ToString();
                row["Time1"] = item["Time1"]?.ToString();
                row["Time2"] = item["Time2"]?.ToString();
                row["Time3"] = item["Time3"]?.ToString();
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
        item.Add("Date", System.DateTime.Now);
        item.Add("Time1", this.form.Level1Time);
        item.Add("Time2", this.form.Level2Time);
        item.Add("Time3", this.form.Level3Time);
        json.Add(item);
        string jsonData = json.ToString();
        File.WriteAllText(jsonFilePath, jsonData);
    }
}
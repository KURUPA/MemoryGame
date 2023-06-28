using System.Data;
using Newtonsoft.Json.Linq;
using MemoryGame.Tabs;


namespace MemoryGame
{
    public partial class MainForm
    {
        public static DataTable songDataTable = createDataTable();

        public TabControl tabControl = new TabControl();
        public MainMenu menu;
        public Description1 description1;
        public Level1 level1;
        public Description2 description2;
        public Level2 level2;
        public Description3 description3;
        public Level3 level3;
        public Scoreboard scoreboard;
        public TimeSpan Level1Time;
        public TimeSpan Level2Time;
        public TimeSpan Level3Time;

        private void InitializeCard()
        {
            this.Controls.Add(tabControl);
            tabControl.Location = new Point(0, 0);
            tabControl.Size = this.ClientSize;
            tabControl.TabPages.Add(menu);
            tabControl.TabPages.Add(description1);
            tabControl.TabPages.Add(level1);
            tabControl.TabPages.Add(description2);
            tabControl.TabPages.Add(level2);
            tabControl.TabPages.Add(description3);
            tabControl.TabPages.Add(level3);
            tabControl.TabPages.Add(scoreboard);
            tabControl.SelectedIndex = 0;
            tabControl.Appearance = TabAppearance.FlatButtons;
            tabControl.SizeMode = TabSizeMode.Fixed;
            foreach (TabPage tabPage in tabControl.TabPages)
            {
                tabPage.BackColor = Color.Transparent;
            }
            tabControl.Show();

        }

        private static DataTable createDataTable()
        {
            string jsonFilePath = "assets/data/data_table.json";
            string jsonData = File.ReadAllText(jsonFilePath);
            JArray json = JArray.Parse(jsonData);
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Singer");
            dataTable.Columns.Add("Title");
            dataTable.Columns.Add("File");
            foreach (JToken item in json)
            {
                if (item["Singer"] != null && item["Title"] != null && item["File"] != null)
                {
                    DataRow row = dataTable.NewRow();
                    row["Singer"] = item["Singer"]?.ToString();
                    row["Title"] = item["Title"]?.ToString();
                    row["File"] = item["File"]?.ToString();
                    dataTable.Rows.Add(row);
                }
            }
            return dataTable;
        }
        public static string FindTextByKeyAndType(string key, string value)
        {
            DataRow[] rows = songDataTable.Select($"File = '{key}'");
            if (rows.Length > 0)
            {
                return (string)rows[0][value];
            }
            else
            {
                return value + " not found";
            }
        }

        public void updataTime()
        {
            JObject jObject = new JObject();
            jObject.Add("Date", System.DateTime.Today);
            jObject.Add("Level_1_Time", this.Level1Time);
            jObject.Add("Level_2_Time", this.Level2Time);
            jObject.Add("Level_3_Time", this.Level3Time);
            string jsonFilePath = "assets/data/scoreboard.json";
            string jsonData = File.ReadAllText(jsonFilePath);
            JArray jArray;
            if (jsonData != null)
            {
                jArray = JArray.Parse(jsonData);
            }
            else
            {
                jArray = new JArray();
            }
            jArray.Add(jObject);
            string filePath = "assets/data/scoreboard.json";
            File.WriteAllText(filePath, jArray.ToString());
        }
    }
}
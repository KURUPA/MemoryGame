using System.Data;
using Newtonsoft.Json.Linq;
using MemoryGame.Tabs;


namespace MemoryGame
{
    public partial class MainForm
    {
        public static DataTable songDataTable = CreateDataTable();

        public TabControl tabControl = new TabControl();
        public MainMenu menu;
        public TimeSpan Level1Time;
        public TimeSpan Level2Time;
        public TimeSpan Level3Time;

        private void InitializeCard()
        {
            this.Controls.Add(tabControl);
            tabControl.Location = new Point(0, -72);
            tabControl.Size = this.ClientSize;
            //在tabControl加入主畫面
            tabControl.TabPages.Add(menu);
            //在tabControl加入第一關說明頁
            tabControl.TabPages.Add(new Description(tabControl, "播放音樂後，根據內容從選項中選取正確的歌名與歌手。", 2));
            //在tabControl加入第一關
            tabControl.TabPages.Add(new Level1(tabControl, menu));

            tabControl.SelectedIndex = 0;
            tabControl.Appearance = TabAppearance.FlatButtons;
            tabControl.SizeMode = TabSizeMode.Fixed;
            foreach (TabPage tabPage in tabControl.TabPages)
            {
                tabPage.BackColor = Color.DarkSlateGray;
            }
            tabControl.Show();

        }

        private static DataTable CreateDataTable()
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

        /*
        public void UpdataTime()
        {
            JObject jObject = new JObject
            {
                { "Date", System.DateTime.Today },
                { "Level_1_Time", this.Level1Time },
                { "Level_2_Time", this.Level2Time },
                { "Level_3_Time", this.Level3Time }
            };
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
        }*/
    }
}
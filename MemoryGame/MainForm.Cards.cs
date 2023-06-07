using System.Data;
using Newtonsoft.Json.Linq;


namespace MemoryGame
{
    public partial class MainForm
    {
        public static DataTable LangDataTable = createLangDataTable();

        public TabControl tabControl = new TabControl();
        public TabPage menu;
        public TabPage description1;
        public TabPage level1;
        public TabPage description2 = new TabPage("Description 2");
        public TabPage level2 = new TabPage("Level 2");
        public TabPage description3 = new TabPage("Description 3");
        public TabPage level3 = new TabPage("Level 3");

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
            tabControl.SelectedIndex = 0;
            tabControl.Show();
        }




        private static DataTable createLangDataTable()
        {
            string jsonFilePath = "assets/data/data_table.json";
            string jsonData = File.ReadAllText(jsonFilePath);
            JArray json = JArray.Parse(jsonData);
            DataTable dataTable = new DataTable();
            // 建立DataTable的欄位
            dataTable.Columns.Add("Singer");
            dataTable.Columns.Add("Title");
            dataTable.Columns.Add("File");
            foreach (JToken item in json)
            {
                DataRow row = dataTable.NewRow();
                row["Singer"] = item["Singer"]?.ToString();
                row["Title"] = item["Title"]?.ToString();
                row["File"] = item["File"]?.ToString();
                dataTable.Rows.Add(row);
            }
            return dataTable;
        }
        public static string FindTextByKeyAndLang(DataTable dataTable, string key, string value)
        {
            DataRow[] rows = dataTable.Select($"File = '{key}'");
            if (rows.Length > 0)
            {
                return (string)rows[0][value];
            }
            else
            {
                return value + " not found";
            }
        }
    }
}
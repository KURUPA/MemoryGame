using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace MemoryGame
{
    public partial class MainForm
    {
        private CardManager cardManager;
        public static DataTable LangDataTable = createLangDataTable();


        private void InitializeCard()
        {


        }

        private void ButtonStartClick(object sender, EventArgs e)
        {
            SuspendLayout();
            setStartVisible(false);
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    SongTitle card = CreateCard(20 + j * (6 + SongTitle.CARD_WIDTH), 20 + i * (6 + SongTitle.CARD_HEIGHT), i * 10 + j, "cat");
                    cardManager.AddCard(card);
                }
            }
            cardManager.RandomlyAssignKeys();
            cardManager.CardList.ForEach(card => this.Controls.Add(card));
            ResumeLayout();
        }

        private SongTitle CreateCard(int x, int y, int index, String key)
        {
            return new SongTitle(x, y, this.cardManager, key, index);
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

        public void setStartVisible(bool visible)
        {
            this.ButtonStart.Visible = visible;
            this.ButtonStart.Enabled = visible;
        }
    }
}
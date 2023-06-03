using System.Data;
using System.IO;
using System.Text.Json.Nodes;
using Newtonsoft.Json;


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
            cardManager.Lang1 = comboBoxLang1.SelectedValue != null ? (string)comboBoxLang1.SelectedValue : "zh_tw";
            cardManager.Lang2 = comboBoxLang2.SelectedValue != null ? (string)comboBoxLang2.SelectedValue : "en_us";
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    Card card = CreateCard(20 + j * (6 + Card.CARD_WIDTH), 20 + i * (6 + Card.CARD_HEIGHT), i * 10 + j, "cat");
                    cardManager.AddCard(card);
                }
            }
            cardManager.RandomlyAssignKeys();
            cardManager.CardList.ForEach(card => this.Controls.Add(card));
            ResumeLayout();
        }

        private Card CreateCard(int x, int y, int index, String key)
        {
            return new Card(x, y, this.cardManager, key, index);
        }

        private static DataTable createLangDataTable()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("Song", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Text", typeof(string)));
            var data = new[]
            {
            new { Song = "key.dog", Text = "Dog" },
            new { Song = "key.dog", Text = "狗" },
            };
            foreach (var item in data)
            {
                var row = dataTable.NewRow();
                row["Song"] = item.Song;
                row["Text"] = item.Text;
                dataTable.Rows.Add(row);
            }
            string filePath = "assets/data/data_table.json";
            string json = JsonConvert.SerializeObject(dataTable, Formatting.Indented);
            File.WriteAllText(filePath, json);
            return dataTable;
        }
        public static string FindTextByKeyAndLang(DataTable dataTable, string key, string lang)
        {
            DataRow[] rows = dataTable.Select($"Key = '{key}' AND Lang = '{lang}'");
            if (rows.Length > 0)
            {
                return (string)rows[0]["Text"];
            }
            else
            {
                return "Text not found";
            }
        }

        public void setStartVisible(bool visible)
        {
            this.ButtonStart.Visible = visible;
            this.comboBoxLang1.Visible = visible;
            this.comboBoxLang2.Visible = visible;
            this.ButtonStart.Enabled = visible;
            this.comboBoxLang1.Enabled = visible;
            this.comboBoxLang2.Enabled = visible;
        }
    }
}
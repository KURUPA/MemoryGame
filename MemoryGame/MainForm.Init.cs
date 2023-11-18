using System.Data;
using Newtonsoft.Json.Linq;
using MemoryGame.Tabs;

namespace MemoryGame
{
    
        /// <summary>
        /// 主視窗初始化與設定
        /// </summary>
    public partial class MainForm
    {
        /// <summary>
        /// 用於存儲音樂列表的 DataTable。
        /// </summary>
        public static DataTable songDataTable = CreateDataTable();

        /// <summary>
        /// 主應用程序的 TabControl 控制項。
        /// </summary>
        public TabControl tabControl = new TabControl();

        /// <summary>
        /// 主菜單 MainMenu 的實例。
        /// </summary>
        public MainMenu menu;

        /// <summary>
        /// 初始化主界面。
        /// </summary>
        private void InitializeCard()
        {
            this.Controls.Add(tabControl);
            tabControl.Location = new Point(0, -72);
            tabControl.Size = this.ClientSize;
            // 在tabControl加入主畫面
            tabControl.TabPages.Add(menu);
            // 在tabControl加入遊戲關卡說明頁
            tabControl.TabPages.Add(new Description(tabControl, "播放音樂後，根據內容從選項中選取正確的歌名與歌手。", 2));
            // 在tabControl加入遊戲關卡
            tabControl.TabPages.Add(new Game(tabControl, menu));
            tabControl.SelectedIndex = 0;
            tabControl.Appearance = TabAppearance.FlatButtons;
            tabControl.SizeMode = TabSizeMode.Fixed;

            foreach (TabPage tabPage in tabControl.TabPages)
            {
                tabPage.BackColor = Color.DarkSlateGray;
            }
            tabControl.Show();
        }

        /// <summary>
        /// 從 JSON 文件中讀取音樂數據並創建 DataTable 以儲存它。
        /// </summary>
        /// <returns>包含音樂數據的 DataTable</returns>
        private static DataTable CreateDataTable()
        {
            // 定義 JSON 文件的路徑
            string jsonFilePath = "assets/data/data_table.json";
            // 從 JSON 文件中讀取數據並存儲為字串
            string jsonData = File.ReadAllText(jsonFilePath);
            // 解析 JSON 數據
            JArray json = JArray.Parse(jsonData);
            // 創建一個 DataTable 來存儲音樂數據
            DataTable dataTable = new DataTable();
            // 定義 DataTable 的列，包括 "Singer(歌手)"、"Title(歌名)" 和 "File(檔案路徑)"
            dataTable.Columns.Add("Singer");
            dataTable.Columns.Add("Title");
            dataTable.Columns.Add("File");
            // 遍歷 JSON 數組中的每個元素，並將相應的數據添加到 DataTable 中
            foreach (JToken item in json)
            {
                // 確保 JSON 數據中存在 "Singer"、"Title" 和 "File" 屬性
                if (item["Singer"] != null && item["Title"] != null && item["File"] != null)
                {
                    // 創建一個新的 DataRow 以存儲音樂數據
                    DataRow row = dataTable.NewRow();
                    // 將 "Singer"、"Title" 和 "File" 屬性的值轉換為字串並分配給對應的列
                    row["Singer"] = item["Singer"]?.ToString();
                    row["Title"] = item["Title"]?.ToString();
                    row["File"] = item["File"]?.ToString();
                    // 向 DataTable 添加這一行數據
                    dataTable.Rows.Add(row);
                }
            }
            // 返回包含音樂數據的 DataTable
            return dataTable;
        }


        /// <summary>
        /// 根據文件名和屬性類型查找相關的歌曲信息。
        /// </summary>
        /// <param name="key">文件名</param>
        /// <param name="value">要查找的屬性類型（"Singer(歌手)" 或 "Title(歌名)"）</param>
        /// <returns>歌曲信息</returns>
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
    }
}

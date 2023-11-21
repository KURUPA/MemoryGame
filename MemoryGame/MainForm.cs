namespace MemoryGame;

using MemoryGame.Tabs;
using System.Data;
using Newtonsoft.Json.Linq;

public partial class MainForm : Form
{
    public static DataTable SongDataTable { get; set; } = LoadMusicDataFromJson(); // 儲存音樂列表的 DataTable
    public TabControl TabControl { get; set; } // 主應用程式的 TabControl 控制項
    public MainMenu Menu { get; set; } // 主選單 MainMenu 的實例

    /// <summary>
    /// 初始化主視窗。
    /// </summary>
    public MainForm()
    {
        AutoScaleDimensions = new SizeF(7F, 15F); // 設定自動縮放的維度
        AutoScaleMode = AutoScaleMode.Font; // 設定自動縮放的模式為根據字型進行縮放
        ClientSize = new Size(1920, 1080); // 設定視窗的用戶端大小為 1920x1080
        MaximizeBox = false; // 禁用最大化按鈕，使視窗不能最大化
        Name = "MainForm"; // 設定視窗的名稱為 "MainForm"
        Text = "延緩失智猜歌遊戲"; // 設定視窗的標題為 "延緩失智猜歌遊戲"
        TabControl = new TabControl // 初始化 TabControl
        {
            Location = new Point(0, -72), // 設定位置
            Size = ClientSize, // 設定大小
            SelectedIndex = 0, // 預設選中的索引
            Appearance = TabAppearance.FlatButtons, // 設定標籤外觀
            SizeMode = TabSizeMode.Fixed // 設定標籤大小模式
        };
        Menu = new MainMenu(TabControl); // 創建主選單 MainMenu 的實例，並傳入 TabControl 參數
        MinimumSize = new Size(960, 540); // 設定窗體的最小大小
        Controls.Add(TabControl); // 在窗體控制項集合中加入 TabControl
        TabControl.TabPages.Insert(0, Menu);  // 在 tabControl 中加入主畫面
        TabPage description = new Description(TabControl, "播放音樂後，根據內容從選項中選取正確的歌名與歌手。", 2);
        TabControl.TabPages.Insert(1, description);   // 在 tabControl 中加入遊戲關卡說明頁
        TabControl.Show(); // 顯示 TabControl
    }
    /// <summary>
    /// 從 JSON 文件中讀取音樂數據並創建 DataTable 以儲存它。
    /// </summary>
    /// <returns>包含音樂數據的 DataTable</returns>
    private static DataTable LoadMusicDataFromJson()
    {
        string jsonFilePath = "assets/data/data_table.json"; // 定義 JSON 文件的路徑
        string jsonData = File.ReadAllText(jsonFilePath); // 從 JSON 文件中讀取數據並儲存為字串
        JArray json = JArray.Parse(jsonData); // 解析 JSON 數據
        DataTable dataTable = new(); // 創建一個 DataTable 來儲存音樂數據
        dataTable.Columns.Add("Singer"); dataTable.Columns.Add("Title"); dataTable.Columns.Add("File"); // 定義 DataTable 的列
        foreach (JToken item in json) // 遍歷 JSON 數組中的每個元素，並將相應的數據添加到 DataTable 中
        {
            if (item["Singer"] != null && item["Title"] != null && item["File"] != null) // 確保 JSON 數據中存在 "Singer"、"Title" 和 "File" 屬性
            {
                DataRow row = dataTable.NewRow(); // 創建一個新的 DataRow 以儲存音樂數據
                // 將 "Singer"、"Title" 和 "File" 屬性的值分配給對應的列
                row["Singer"] = item["Singer"]?.ToString();
                row["Title"] = item["Title"]?.ToString();
                row["File"] = item["File"]?.ToString();
                dataTable.Rows.Add(row); // 向 DataTable 添加這一行數據
            }
        }
        return dataTable; // 返回包含音樂數據的 DataTable
    }
    /// <summary>
    /// 根據檔案名和屬性類型查找相關的歌曲資訊。
    /// </summary>
    /// <param name="key">檔案名</param>
    /// <param name="value">要查找的屬性類型（"Singer(歌手)" 或 "Title(歌名)"）</param>
    /// <returns>歌曲資訊</returns>
    public static string GetSongInfoByKey(string key, string value)
    {
        DataRow[] rows = SongDataTable.Select($"File = '{key}'"); // 通過條件查找 DataTable 中的行
        if (rows.Length > 0) return (string)rows[0][value]; // 返回查找到的值
        else return value + " not found"; // 若未找到，返回提示資訊
    }
}

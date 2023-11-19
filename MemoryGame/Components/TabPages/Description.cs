namespace MemoryGame.Tabs;

/// <summary>
/// 描述遊戲關卡的 TabPage，用於顯示遊戲關卡的說明和開始遊戲選項。
/// </summary>
public class Description : TabPage
{
    private readonly Button buttonStart;
    public readonly TabControl tabControl;

    /// <summary>
    /// Description 類別的構造函數，用於初始化描述遊戲關卡的 TabPage。
    /// </summary>
    /// <param name="tabControl">包含描述遊戲關卡的 TabControl</param>
    /// <param name="text">遊戲關卡的說明</param>
    /// <param name="index">選項卡的索引，用於切換至該遊戲關卡</param>
    public Description(TabControl tabControl, string text, int index)
    {
        BorderStyle = BorderStyle.None;
        this.tabControl = tabControl;
        BackColor = Color.DarkSlateGray;
        SuspendLayout();    // 暫停佈局以提高效能
        var Title = new Label   //創建並設定關卡說明標題
        {
            ForeColor = Color.White,
            Size = new Size(800,120),
            Location = new Point((tabControl.Width - 800) / 2, Height),
            Text = text
        };
        this.Controls.Add(Title);   //將標題加入控制項
        this.buttonStart = new Button   // 創建並設定開始關卡按鈕
        {
            Font = MainMenu.GetMicrosoftJhengHeiFont(),
            Name = "buttonStart",
            Size = new Size(300, 90),
            Location = new Point((tabControl.Width - 300) / 2, 261),
            TabIndex = 0,
            Text = "開始關卡",
            TextAlign = ContentAlignment.MiddleCenter,
            UseVisualStyleBackColor = true
        };
        this.buttonStart.Click += (sender, eventArgs) => tabControl.SelectedIndex = index;  
        this.Controls.Add(this.buttonStart);    //將開始按鈕加入控制項
        ResumeLayout(); // 恢復佈局
    }
}

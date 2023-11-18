namespace MemoryGame.Tabs;

/// <summary>
/// 描述遊戲關卡的 TabPage，用於顯示遊戲關卡的說明和開始遊戲選項。
/// </summary>
public class Description : TabPage
{
    private Button buttonStart;
    public TabControl tabControl;

    /// <summary>
    /// Description 類別的構造函數，用於初始化描述遊戲關卡的 TabPage。
    /// </summary>
    /// <param name="tabControl">包含描述遊戲關卡的 TabControl</param>
    /// <param name="value">遊戲關卡的說明</param>
    /// <param name="index">選項卡的索引，用於切換至該遊戲關卡</param>
    public Description(TabControl tabControl, string value, int index)
    {
        Text = "Description";
        BorderStyle = BorderStyle.None;
        this.tabControl = tabControl;
        BackColor = Color.DarkSlateGray;

        SuspendLayout();

        //創建並設定關卡說明標題
        var Title = new Label
        {
            ForeColor = Color.White,
            Width = 800,
            Height = 120,
            Location = new Point((tabControl.Width - 800) / 2, Height),
            Text = value
        };
        this.Controls.Add(Title);

        // 創建並設定開始關卡按鈕
        this.buttonStart = new Button
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
        this.Controls.Add(this.buttonStart);

        ResumeLayout();
    }
}

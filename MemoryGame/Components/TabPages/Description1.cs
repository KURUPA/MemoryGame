namespace MemoryGame.Tabs;

public class Description1 : TabPage
{

    private Button buttonStart;
    private Label labelTitle;
    public TabControl tabControl;
    private Size formClientSize;
    public Description1(TabControl tabControl, Size formClientSize)
    {
        this.Text = "Description1";
        this.tabControl = tabControl;
        this.formClientSize = formClientSize;
        SuspendLayout();
        this.labelTitle = new Label();
        this.labelTitle.Font = MainMenu.getFont();
        this.labelTitle.Name = "labelTitle";
        this.labelTitle.Size = new Size(420, 30);
        this.labelTitle.Location = new Point((formClientSize.Width - labelTitle.Width) / 2, 68);
        this.labelTitle.TabIndex = 1;
        this.labelTitle.Text = "播放音樂後，從選項中選取正確的歌名與歌手。";

        this.buttonStart = new Button();
        this.buttonStart.Font = MainMenu.getFont();
        this.buttonStart.Name = "buttonStart";
        this.buttonStart.Size = new Size(75, 23);
        this.buttonStart.Location = new Point((formClientSize.Width - buttonStart.Width) / 2, 261);
        this.buttonStart.TabIndex = 0;
        this.buttonStart.Text = "START";
        this.buttonStart.UseVisualStyleBackColor = true;
        this.buttonStart.Click += (s, e) => tabControl.SelectedIndex = 2;

        this.Controls.Add(buttonStart);
        this.Controls.Add(labelTitle);

        ResumeLayout();
    }

}
namespace MemoryGame.Tabs;
using System.Data;
using Newtonsoft.Json.Linq;

public class Scoreboard : TabPage
{
    MainForm form;
    public Scoreboard(TabControl tabControl, MainForm form)
    {
        this.form = form;
        this.Text = "Description1";
        this.BorderStyle = BorderStyle.None;
        this.BackgroundImage = Image.FromFile("assets/texture/background.png");
        PictureBox back = new PictureBox();
        back.Size = new Size(600, 300);
        back.Image = Image.FromFile("assets/texture/scoreboard.png");
        back.Location = new Point((this.form.Width - back.Width) / 2, (this.form.Height - back.Height) / 2);
        this.Controls.Add(back);

    }

}
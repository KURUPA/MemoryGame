using System.Data;
using Timer = System.Windows.Forms.Timer;

namespace MemoryGame
{
    public class SongTitleManager
    {
        public int point;
        private readonly Timer DelayTimer;
        public List<SongTitle> list { get; set; }
        private string CardTitleFile { get; set; }
        private string song { get; set; }
        public bool CanPick = true;
        public Managerlistener? managerlistener;
        public SongTitle? pickSong;

        public SongTitleManager()
        {
            list = new List<SongTitle>();
            CardTitleFile = "";
            song = "";
            DelayTimer = new Timer();
            DelayTimer.Interval = 500;
            DelayTimer.Tick += new EventHandler(DelayTimer_Tick);
        }

        public void AddSongTitle(SongTitle card)
        {
            list.Add(card);
        }

        public void setSong(string File)
        {
            this.song = File;
        }

        public void ClearAllCard()
        {
            list.Clear();
        }

        public void RandomlyAssignKeys()
        {
            Random random = new Random();
            list = this.list.OrderBy(card => random.Next()).ToList();
            List<string> keys = new List<string>();
            foreach (DataRow row in MainForm.songDataTable.Rows)
            {
                string? file = row["File"].ToString();
                if (file != null)
                {
                    keys.Add(file);
                }
            }
            keys = keys.OrderBy(x => random.Next()).ToList();
            if (keys.Count < list.Count)
            {
                Console.WriteLine("not enough File keys :" + keys.ToString());
                return;
            }
            for (int i = 0; i < list.Count; i++)
            {
                string key = keys[i];
                list[i].File = key;
                list[i].FlipOver(false);
            }
        }

        public string getRandomSong()
        {
            string song = list.ElementAt(new Random().Next(list.Count)).File;
            return song;
        }

        public Boolean isCanPick() { return this.CanPick; }

        public void PickCard(SongTitle song)
        {
            this.CanPick = false;
            this.pickSong = song;
            DelayTimer.Start();
        }

        private void DelayTimer_Tick(object? sender, EventArgs e)
        {
            bool match = false;
            CanPick = true;
            if (pickSong == null)
            {
                return;
            }
            pickSong.FlipOver(false);
            pickSong.Image = pickSong.IsShowText ? SongTitle.ButtonLightImage : SongTitle.ButtonImage;
            if (pickSong.File == song)
            {
                pickSong.Visible = false;
                pickSong.Enabled = false;
                list.Remove(pickSong);
                match = true;
                point++;
            }
            CardTitleFile = "";
            if (managerlistener != null)
            {
                managerlistener.CardPick(pickSong, match);
            }
            DelayTimer.Stop();
        }
    }

}
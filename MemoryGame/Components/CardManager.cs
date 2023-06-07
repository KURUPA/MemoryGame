using System.Data;
using Timer = System.Windows.Forms.Timer;

namespace MemoryGame
{
    public class CardManager
    {
        public int point;
        private readonly Timer DelayTimer;
        private readonly Timer Tick;
        public List<SongTitle> CardList { get; set; }
        private int CardTitleId { get; set; }
        private string song { get; set; }
        private bool CanPick = true;

        public CardManager()
        {
            CardList = new List<SongTitle>();
            CardTitleId = -1;
            song = "";
            DelayTimer = new Timer();
            Tick = new Timer();
            DelayTimer.Interval = 500;
            Tick.Interval = 20;
            DelayTimer.Tick += new EventHandler(DelayTimer_Tick);
        }

        public void AddCard(SongTitle card)
        {
            CardList.Add(card);
        }

        public void setSong(string File)
        {
            this.song = File;
        }

        public void ClearAllCard()
        {
            CardList.Clear();
        }

        public void RandomlyAssignKeys()
        {
            Random random = new Random();
            CardList = CardList.OrderBy(card => random.Next()).ToList();
            List<string> keys = new List<string>();
            foreach (DataRow row in MainForm.LangDataTable.Rows)
            {
                string? file = row["File"].ToString();
                if (file != null)
                {
                    keys.Add(file);
                }
            }
            keys = keys.OrderBy(x => random.Next()).ToList();
            if (keys.Count < CardList.Count)
            {
                Console.WriteLine("沒有足夠的File keys :" + keys.ToString());
                return;
            }
            for (int i = 0; i < CardList.Count; i++)
            {
                string key = keys[i];
                CardList[i].File = key;
            }
        }

        public SongTitle? getCardTitleId()
        {
            return this.GetCardById(this.CardTitleId);
        }

        public string getSong()
        {
            string song = CardList.ElementAt(new Random().Next(CardList.Count)).File;
            return song;
        }

        public Boolean isCanPick() { return this.CanPick; }

        public void PickCard(SongTitle card)
        {
            this.CanPick = false;
            this.CardTitleId = card.Id;
            DelayTimer.Start();

        }

        public SongTitle? GetCardById(int id)
        {
            return CardList.FirstOrDefault(card => card.Id == id);
        }
        private void DelayTimer_Tick(object? sender, EventArgs e)
        {
            SongTitle? cardFirst = getCardTitleId();
            string song = getSong();
            if (cardFirst == null)
            {
                return;
            }
            cardFirst.FlipOver(false);
            if (cardFirst.File == song)
            {
                cardFirst.Visible = false;
                cardFirst.Enabled = false;
                point++;
            }
            CardTitleId = -1;
            CanPick = true;
            DelayTimer.Stop();
        }
    }

}
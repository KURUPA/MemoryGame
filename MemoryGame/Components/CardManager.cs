using System.Data;
using Timer = System.Windows.Forms.Timer;

namespace MemoryGame
{
    public class CardManager
    {
        private readonly Timer DelayTimer;
        private readonly Timer Tick;
        public List<SongTitle> CardList { get; set; }
        private int CardTitleId { get; set; }
        private int SongId { get; set; }
        private bool isPickOne { get; set; }
        private bool CanPick = true;

        public CardManager()
        {
            CardList = new List<SongTitle>();
            CardTitleId = -1;
            SongId = -1;
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
            for (int i = 0; i < CardList.Count; i ++)
            {
                string key = keys[i];
                CardList[i].File = key;
            }
        }

        public SongTitle? getCardTitleId()
        {
            return this.GetCardById(this.CardTitleId);
        }

        public SongTitle? getSongId()
        {
            return this.GetCardById(this.SongId);
        }

        public void ResetPickCard()
        {
            CardTitleId = -1;
            SongId = -1;
            isPickOne = false;
            CanPick = true;
        }

        public Boolean isCanPick() { return this.CanPick; }

        public void PickCard(SongTitle card)
        {
            if (!isPickOne)
            {
                CardTitleId = card.Id;
                isPickOne = true;
            }
            else
            {
                SongId = card.Id;
                CanPick = false;
                DelayTimer.Start();
            }
        }

        public SongTitle? GetCardById(int id)
        {
            return CardList.FirstOrDefault(card => card.Id == id);
        }
        private void DelayTimer_Tick(object? sender, EventArgs e)
        {
            SongTitle? cardFirst = getCardTitleId();
            SongTitle? cardSecondId = getSongId();
            if (cardFirst == null || cardSecondId == null) { return; }
            if (cardFirst.File == cardSecondId.File)
            {
                cardFirst.Visible = false;
                cardSecondId.Visible = false;
            }
            else
            {
                cardFirst.FlipOver(false);
                cardSecondId.FlipOver(false);
            }
            ResetPickCard();
            isPickOne = false;
            DelayTimer.Stop();
        }
    }

}
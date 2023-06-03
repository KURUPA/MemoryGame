using System.Data;
using Timer = System.Windows.Forms.Timer;

namespace MemoryGame
{
    public class CardManager
    {
        private readonly Timer DelayTimer;
        private readonly Timer Tick;
        public List<Card> CardList { get; set; }
        private int CardFirstId { get; set; }
        private int CardSecondId { get; set; }
        public string Lang1 { get; set; }
        public string Lang2 { get; set; }
        private bool isPickOne { get; set; }
        private bool CanPick = true;

        public CardManager()
        {
            Lang1 = "zh_tw";
            Lang2 = "en_us";
            CardList = new List<Card>();
            CardFirstId = -1;
            CardSecondId = -1;
            DelayTimer = new Timer();
            Tick = new Timer();
            DelayTimer.Interval = 500;
            Tick.Interval = 20;
            DelayTimer.Tick += new EventHandler(DelayTimer_Tick);
        }

        public void AddCard(Card card)
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
                string? key = row["Key"].ToString();
                if (key != null)
                {
                    keys.Add(key);
                }
            }
            keys = keys.OrderBy(x => random.Next()).ToList();
            if (keys.Count < CardList.Count / 2)
            {
                Console.WriteLine("沒有足夠的keys :" + keys.ToString());
                return;
            }
            for (int i = 0; i < CardList.Count; i += 2)
            {
                string key = keys[i / 2];
                CardList[i].Key = key;
                CardList[i].Lang = Lang1;
                CardList[i + 1].Key = key;
                CardList[i + 1].Lang = Lang2;
            }
        }

        public Card? getCardFirst()
        {
            return this.GetCardById(this.CardFirstId);
        }

        public Card? getCardSecond()
        {
            return this.GetCardById(this.CardSecondId);
        }

        public void ResetPickCard()
        {
            CardFirstId = -1;
            CardSecondId = -1;
            isPickOne = false;
            CanPick = true;
        }

        public Boolean isCanPick() { return this.CanPick; }

        public void PickCard(Card card)
        {
            if (!isPickOne)
            {
                CardFirstId = card.Id;
                isPickOne = true;
            }
            else
            {
                CardSecondId = card.Id;
                CanPick = false;
                DelayTimer.Start();
            }
        }

        public Card? GetCardById(int id)
        {
            return CardList.FirstOrDefault(card => card.Id == id);
        }
        private void DelayTimer_Tick(object? sender, EventArgs e)
        {
            Card? cardFirst = getCardFirst();
            Card? cardSecondId = getCardSecond();
            if (cardFirst == null || cardSecondId == null) { return; }
            if (cardFirst.Key == cardSecondId.Key)
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
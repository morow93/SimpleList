using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticsearchTest.Services
{
    public class Card
    {
        public Card(Card card)
        {
            if (card != null)
            {
                this.Id = card.Id;
                this.Title = card.Title;
                this.Text = card.Text;
            }
        }

        public Int32 Id { get; set; }
        public String Title { get; set; }
        public String Text { get; set; }
    }

    public class RootObject
    {
        public List<Card> Cards { get; set; }
    }

    public class CardsService
    {
        private static List<Card> _cards = null;
        static CardsService()
        {
            var res = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText("cards.json"));
            _cards = res.Cards.ToList();
        }
                
        public List<Card> Cards()
        {
            return _cards;
        }

        public Card Get(Int32 id)
        {
            if (_cards != null && _cards.Count != 0)
            {
                return new Card(_cards.FirstOrDefault(c => c.Id == id));
            }
            return null;
        }

        public Card Update(Card card)
        {
            if (_cards != null && _cards.Count != 0)
            {
                var updateCard = _cards.FirstOrDefault(c => c.Id == card.Id);
                if (updateCard != null)
                {
                    updateCard.Title = card.Title;
                    updateCard.Text = card.Text;
                    return new Card(updateCard);
                }
            }
            return null;
        }

        public Card Delete(Int32 id)
        {
            if (_cards != null && _cards.Count != 0)
            {
                var card = _cards.FirstOrDefault(c => c.Id == id);
                if (card != null)
                {
                    _cards.Remove(card);
                    return new Card(card);
                }
            }
            return null;
        }

        public Card Add(Card card)
        {
            if (_cards != null && _cards.Count != 0)
            {
                card.Id = _cards.Select(c => c.Id).Max() + 1;
            }
            else
            {
                _cards = new List<Card>();
                card.Id = 1;
            }
            _cards.Add(card);
            return new Card(card);
        }
    }
}

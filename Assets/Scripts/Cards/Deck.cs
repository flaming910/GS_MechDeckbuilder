using MDB.Interfaces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MDB.Cards
{
    public class Deck
    {
        private Stack<CardBase> deck;
        private Stack<CardBase> discard;

        public int DeckCount => deck.Count;
        public int DiscardCount => discard.Count;

        public Deck(CardBase[] cards)
        {
            deck = new Stack<CardBase>(cards.OrderBy(x => Random.value));
            discard = new Stack<CardBase>();
        }

        /// <summary>
        /// If there are cards in the Deck, move a card from Deck to hand. If there aren't, check if there are cards in Discard pile.
        /// If so, return them to deck and draw a card.
        /// </summary>
        public CardBase Draw()
        {
            if(deck.Count > 0)
            {
                return deck.Pop();
            }
            else
            {
                if (discard.Count > 0)
                {
                    MoveDiscardToDeck();
                    Debug.Log("Now returning Discard Pile to Deck");
                    return deck.Pop();
                }
                else
                {
                    Debug.Log("There are no cards in Deck or Discard pile");
                    return null;
                }
            }
        }

        /// <summary>
        /// If there are cards in the Deck, move some cards from Deck to hand. If there aren't, check if there are cards in Discard pile.
        /// If so, return them to deck and draw a card.
        /// </summary>
        /// <param name="cardsToDraw"> Number of cards to add to hand </param>
        public List<CardBase> Draw(int cardsToDraw)
        {
            var cards = new List<CardBase>();
            for (int i = 0; i < cardsToDraw; i++)
            {
                if(deck.Count > 0)
                {
                    cards.Add(deck.Pop());
                }
                else
                {
                    if (discard.Count > 0)
                    {
                        MoveDiscardToDeck();
                        Debug.Log("Now returning Discard Pile to Deck");
                        cards.Add(deck.Pop());
                    }
                    else
                    {
                        Debug.Log("There are no cards in Deck or Discard pile");
                    }
                }
            }

            return cards;
        }

        /// <summary>
        /// Add a card to the Discard Pile
        /// </summary>
        /// <param name="card"> Card to be added to Discard </param>
        public void SendToDiscard(CardBase card)
        {
            discard.Push(card);
        }

        public void Shuffle()
        {
            deck = new Stack<CardBase>(deck.OrderBy(x => Random.value));
        }

        /// <summary> Shuffle all cards in Discard Pile to the Deck, also clear the Discard Pile </summary>
        public void MoveDiscardToDeck()
        {
            deck = new Stack<CardBase>(discard);
            discard.Clear();
            Shuffle();
        }

        /// <summary>
        /// Get an array of cards in draw pile
        /// </summary>
        /// <returns> Cards in draw pile, as array </returns>
        public CardBase[] GetDeck()
        {
            return deck.ToArray();
        }

        /// <summary>
        /// Get an array of cards in discard pile
        /// </summary>
        /// <returns> Cards in discard pile, as array </returns>
        public CardBase[] GetDiscard()
        {
            return discard.ToArray();
        }

        #region For now, these aren't implemented cause it's cleaner to use UI button for click in this specific case
        /*
        /// <summary> When mouse gets over deck icon, display number of cards in deck? </summary>
        public void OnPointerEnter(PointerEventData eventData)
        {

        }

        /// <summary> When mouse moves away from deck icon, hide number of cards in deck? </summary>
        public void OnPointerExit(PointerEventData eventData)
        {

        }

        /// <summary> When mouse clicks deck icon, show cards in deck, preferrably in random order?? Rarity? Cost? Alphabet? </summary>
        public void OnPointerClick(PointerEventData eventData)
        {

        }
        */
        #endregion
    }
}

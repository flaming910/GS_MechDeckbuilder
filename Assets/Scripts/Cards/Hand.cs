using System.Collections.Generic;
using UnityEngine;
using MDB.Controllers;

namespace MDB.Cards
{
    /// <summary> Class of making hands for characters. Hands have cards in them, characters can play cards from their hand, and hands get discarded at end of turn. </summary>
    public class Hand
    {
        /// <summary> List of cards currently in hand </summary>
        private List<CardInstance> handCards;

        /// <summary> Hand transform </summary>
        private Transform handTransform;

        /// <summary> The physical card template </summary>
        private GameObject cardPrefab;

        /// <summary> Just for debugging. Getting a random card from the card database to test adding it to hand </summary>
        private CardBase temporaryCard;

        private bool isPlayerHand;

        public List<CardInstance> HandCards => handCards;

        public Hand(Transform handTransform, GameObject cardPrefab, bool isPlayerHand)
        {
            this.handTransform = handTransform;
            this.cardPrefab = cardPrefab;
            this.isPlayerHand = isPlayerHand;
            handCards = new List<CardInstance>();
        }

        /// <summary> Create the card in hand, set up its visuals, and add it to the list, also keep a reference to its controller </summary>
        public void AddCard(CardBase card, ControllerBase control)
        {
            GameObject gc = Object.Instantiate(cardPrefab, Vector3.zero, Quaternion.identity, handTransform);

            CardInstance cardInstance = gc.GetComponent<CardInstance>();
            cardInstance.SetUpCard(card, control, isPlayerHand);

            handCards.Add(cardInstance);
        }

        public void AddCard(List<CardBase> cards, ControllerBase control)
        {
            foreach (var card in cards)
            {
                GameObject gc = Object.Instantiate(cardPrefab, Vector3.zero, Quaternion.identity, handTransform);

                CardInstance cardInstance = gc.GetComponent<CardInstance>();
                cardInstance.SetUpCard(card, control, isPlayerHand);

                handCards.Add(cardInstance);
            }
        }

        /// <summary> Remove card from the list and destroy its physical instance </summary>
        public void RemoveCard(CardInstance card)
        {
            handCards.Remove(card);
            Object.Destroy(card.gameObject);
        }

        /// <summary> Empties the hand out(list and destroys gameobjects) </summary>
        public void EmptyHand()
        {
            foreach (var card in handCards)
            {
                Object.Destroy(card.gameObject);
            }
            handCards = new List<CardInstance>();
        }
    }
}

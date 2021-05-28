using MDB.Cards;
using System.Collections.Generic;
using UnityEngine;

namespace MDB.Managers
{
    /// <summary> The panel that shows rewards. Mostly used after battles to offer cards (gold and body parts?) to player to choose from </summary>
    public class RewardsPanel : MonoBehaviour
    {
        /// <summary> Singleton instance of the reward panel </summary>
        private static RewardsPanel instance;

        /// <summary> Singleton instance of the reward panel </summary>
        public static RewardsPanel Instance
        {
            get => instance;
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        #pragma warning disable 0649
        /// <summary> List of possible card rewards </summary>
        [SerializeField]
        private CardListBase possibleCardDrops;

        /// <summary> The physical card template </summary>
        [SerializeField]
        private GameObject cardPrefab;
        #pragma warning restore 0649


        /// <summary> The currently offered cards </summary>
        private List<CardInstance> offeredCards;


        /// <summary>
        /// Randomly choose a number of cards to offer as a reward
        /// NOTICE: Card prefab should be changed somehow. Currently used prefab PLAYS CARDS when clicked. Gotta be changed to add to deck?
        /// </summary>
        /// <param name="amount"> The number of cards to offer </param>
        public void RollCardRewards(int amount)
        {
            //Roll rarity
            CardRarity rarityToRoll;
            int rarityVal = Random.Range(0, 100);
            if (rarityVal < 40)
            {
                rarityToRoll = CardRarity.Common;
            }
            else if (rarityVal < 70)
            {
                rarityToRoll = CardRarity.Uncommon;
            }
            else if (rarityVal < 90)
            {
                rarityToRoll = CardRarity.Epic;
            }
            else
            {
                rarityToRoll = CardRarity.Legendary;
            }

            var cardsToBeOffered = new List<CardBase>();
            var cardsThatCanBeOffered = possibleCardDrops.GetCardsOfRarity(rarityToRoll);
            offeredCards = new List<CardInstance>();

            if (amount > cardsThatCanBeOffered.Count) amount = cardsThatCanBeOffered.Count;

            print(rarityToRoll.ToString());
            for (int i = 0; i < amount; i++)
            {
                CardBase cardToBeMade = null;
                while (!cardsToBeOffered.Contains(cardToBeMade))
                {
                    cardToBeMade = cardsThatCanBeOffered[Random.Range(0, cardsThatCanBeOffered.Count)];
                    if (!cardsToBeOffered.Contains(cardToBeMade))
                    {
                        cardsToBeOffered.Add(cardToBeMade);
                    }
                }


                GameObject go = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity, transform);

                CardInstance cardInstance = go.GetComponent<CardInstance>();
                cardInstance.SetUpCard(cardToBeMade, GameManager.Instance.PlayerController, true);

                offeredCards.Add(cardInstance);
            }
        }

        /// <summary> Remove all offered card rewards. To be used after player chooses one of the cards to add (or maybe they have to choose between a card or a body part or something?) </summary>
        public void ClearCardRewards()
        {
            foreach (var card in offeredCards)
            {
                if(card.gameObject.activeSelf) Destroy(card.gameObject);
            }

            offeredCards.Clear();
        }

        /// <summary> Clear the rewards screen then return to map </summary>
        public void CloseRewards()
        {
            ClearCardRewards();

            UIManager.Instance.ReturnToMap();
        }
    }
}

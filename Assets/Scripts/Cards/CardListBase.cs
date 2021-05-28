using System;
using System.Collections.Generic;
using MDB.Characters;
using MDB.Managers;
using UnityEngine;

namespace MDB.Cards
{

    public enum CardFunctions
    {
        Damage,
        Heal,
        GainEnergy,
        GainArmor
    }


    [Serializable]
    public struct CardEvent
    {
        public CardFunctions func;
        public int value;
        public TargetType targetType;
    }

    [Serializable]
    public struct Card
    {
        public string cardName;
        public Sprite cardArt;
        public CardRarity rarity;
        [TextArea(1, 6)] public string cardDesc;
        public int cost;
        [ArrayElementTitle("func")] public CardEvent[] cardEvents;
        public BodyPartType sourcePart;
    }

    [CreateAssetMenu(fileName = "CardList", menuName = "MDB/CardList", order = 1)]
    public class CardListBase : ScriptableObject
    {
        #pragma warning disable 0649
        [SerializeField, ArrayElementTitle("cardName")]
        private List<Card> cards;

        [SerializeField]
        private bool isPlayerCards;
        #pragma warning restore 0649
        private List<CardBase> cardList;

        public List<CardBase> CardList => cardList;
        public List<Card> CardsDetailed => cards;


        /// <summary>
        /// Make a card list using just the default rarity cards
        /// </summary>
        /// <param name="cardList"></param>
        /// <returns></returns>
        public void InitialiseCardList(CardListBase cardList)
        {
            this.cards = new List<Card>();
            foreach (var card in cardList.cards)
            {
                if(card.rarity == CardRarity.Default) this.cards.Add(card);
            }

            InitialiseCardList();
        }

        /// <summary>
        /// This creates the card list based on what is in the scriptable object
        /// </summary>
        /// <returns>The card list from the ScriptableObject</returns>
        public void InitialiseCardList()
        {
            cardList = new List<CardBase>();

            //Go through all the cards
            foreach (var card in cards)
            {
                cardList.Add(CardBaseFromCard(card));
            }
        }

        /// <summary>
        /// This will return a CardBase from a Card
        /// </summary>
        /// <param name="card">The card to convert</param>
        /// <returns>The CardBase that can be used in game</returns>
        private CardBase CardBaseFromCard(Card card)
        {
            List<Action> actions = new List<Action>();
            List<TargetType> targets = new List<TargetType>();
            //Go through all the card events
            foreach (var cardEvent in card.cardEvents)
            {
                switch (cardEvent.func)
                {
                    case CardFunctions.Damage:
                        actions.Add( () => CardBase.DealDamage(GetTargetFromType(cardEvent.targetType), cardEvent.value));
                        break;
                    case CardFunctions.Heal:
                        actions.Add(() => CardBase.Heal(GetTargetFromType(cardEvent.targetType), cardEvent.value));
                        break;
                    case CardFunctions.GainEnergy:
                        actions.Add( () => CardBase.GainEnergy(GetTargetFromType(cardEvent.targetType) as Character, cardEvent.value  ));
                        break;
                    case CardFunctions.GainArmor:
                        actions.Add( () => CardBase.GainArmor(GetTargetFromType(cardEvent.targetType) as BodyPart, cardEvent.value  ));
                        break;
                }

                if (cardEvent.targetType != TargetType.None)
                {
                    targets.Add(cardEvent.targetType);
                }
            }
            // ReSharper disable once Unity.IncorrectMonoBehaviourInstantiation
            return new CardBase(
                card.cardName,
                card.cardArt,
                card.rarity,
                card.cardDesc,
                card.cost,
                actions,
                card.sourcePart,
                targets.ToArray()
            );

        }

        /// <summary>
        /// Gets a reference to the target based on the target type
        /// </summary>
        /// <param name="type">Target type to get</param>
        /// <returns>A target</returns>
        private ITarget GetTargetFromType(TargetType type)
        {
            GameManager gm = GameManager.Instance;
            if (type.ToString().Contains("Enemy"))
            {
                if (type == TargetType.EnemyCore)
                {
                    if (gm.CurrentEnemyCharacter.CurrentCoreHealth >= 0) return gm.CurrentEnemyCharacter;
                }
                else
                {
                    return gm.CurrentEnemyCharacter.GetBodyPart(((BodyPartType) Enum.Parse(typeof(BodyPartType), type.ToString().Substring(5))));
                }
            }
            else if (type.ToString().Contains("Player"))
            {
                if (type == TargetType.PlayerCore)
                {
                    if (gm.PlayerCharacter.CurrentCoreHealth >= 0) return gm.PlayerCharacter;
                }
                else
                {
                    return gm.PlayerCharacter.GetBodyPart((BodyPartType)Enum.Parse(typeof(BodyPartType), type.ToString().Substring(6)));
                }
            }

            Debug.LogError("Your card event is supposed to have a type but it doesn't so it throws this error");
            return null;
        }

        /// <summary>
        /// Add a card to this list.
        /// </summary>
        /// <param name="card">The card to add to the list</param>
        public void AddCardToList(CardBase card)
        {
            cardList.Add(card);
        }

        public List<CardBase> GetCardsOfRarity(CardRarity rarity)
        {
            List<CardBase> cards = new List<CardBase>();
            foreach (var card in cardList)
            {
                if (card.Rarity == rarity)
                {
                    cards.Add(card);
                }
            }

            return cards;
        }

    }
}



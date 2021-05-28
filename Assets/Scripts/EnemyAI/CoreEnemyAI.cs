using System.Collections.Generic;
using MDB.Cards;
using MDB.Characters;
using MDB.Controllers;
using UnityEngine;
using MDB.Managers;
using MDB.Utils;

namespace MDB.EnemyAI
{
    public class CoreEnemyAI : MonoBehaviour
    {
        #pragma warning disable 0649
        [SerializeField] private ControllerBase enemyController;
        #pragma warning restore 0649

        private Hand hand;
        private Character enemyCharacter;
        private Character playerCharacter;

        private GameManager gm;
        private List<CardInstance> cardsToPlay;

        private CardListBase cardList;

        private int damagePoints;
        private int healPoints;
        private int coreDamagePoints;
        private int destroyPartPoints;

        // Start is called before the first frame update
        private void Start()
        {
            gm = GameManager.Instance;

            hand = enemyController.MyHand;
            enemyCharacter = enemyController.MySourceCharacter;
            playerCharacter = gm.PlayerCharacter;
            cardList = CardDatabase.Instance.CommonEnemyCards;

            damagePoints = 1;
            healPoints = 1;
            coreDamagePoints = 3;
            destroyPartPoints = 3;

            cardsToPlay = new List<CardInstance>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (enemyController.IsMyTurn)
            {
                if (enemyController.MySourceCharacter.CurrentEnergy > 0)
                {
                    cardsToPlay = DetermineTurn(hand.HandCards, 0, 0);
                    if (cardsToPlay == null)
                    {
                        gm.EndTurn();
                    }
                    else
                    {
                        PlayCards();
                    }
                }
                else
                {
                    gm.EndTurn();
                }
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                cardsToPlay = DetermineTurn(hand.HandCards, 0, 0);
                PlayCards();
            }

        }

        private void PlayCards()
        {
            foreach (var card in cardsToPlay)
            {
                Debug.Log("Playing the card: " + card.CardInfo.CardName);
                enemyController.TryPlayCard(card);
            }
        }

        /*
         * Get first card from hand and see if its playable
         * if no: go to next card
         * if yes: calculate the score it grants and also remove the energy cost from the 'energy pool'
         *
         * get first card from trimmed hand and see if its playable
         * if no: go to next card
         * if yes: ...
         *
         *
         */


        //TODO: Make this actually choose multiple cards instead of just the one

        /// <summary>
        /// Goes through hand and picks the best cards to play in the current scenario
        /// </summary>
        /// <returns>Returns list of cards to play</returns>
        private List<CardInstance> PickBestCards()
        {
            int highScore = 0;

            int index = 0;
            List<CardInstance> finalCardsToPlay = new List<CardInstance>();

            foreach (var card in hand.HandCards)
            {
                CharacterObject currEnemyIteration = new CharacterObject(enemyCharacter);
                CharacterObject currPlayerIteration = new CharacterObject(playerCharacter);

                List<CardInstance> cards = new List<CardInstance>(hand.HandCards);
                cards.RemoveAt(index);
                if (card.CardInfo.CardCost <= currEnemyIteration.currentEnergy)
                {
                    int currentScore = 0;
                    List<CardInstance> currentCardsToPlay = new List<CardInstance> {card};
                    Card cardDetailed = cardList.CardsDetailed.Find(i => i.cardName == card.CardInfo.CardName);

                    //Go through all the card functions and add to the score
                    currentScore = CalculateCardScore(cardDetailed, currEnemyIteration, currPlayerIteration);
                    currEnemyIteration.currentEnergy -= card.CardInfo.CardCost;
                    if (currentScore > highScore)
                    {
                        finalCardsToPlay = currentCardsToPlay;
                        highScore = currentScore;
                    }
                }

                index++;
            }

            return finalCardsToPlay;
        }

        /// <summary>
        /// Takes a card and returns the calculated score for the card, also modifies the energy pool if needed
        /// </summary>
        /// <param name="cardDetailed">Card to check</param>
        /// <param name="currEnemyIteration">Reference to the current enemy iteration</param>
        /// /// <param name="currPlayerIteration">Reference to the current player iteration</param>
        /// <returns></returns>
        private int CalculateCardScore(Card cardDetailed, CharacterObject currEnemyIteration, CharacterObject currPlayerIteration)
        {
            int cardScore = 0;
            foreach (var cardEvent in cardDetailed.cardEvents)
            {
                switch (cardEvent.func)
                {
                    //Calculate score for damage function
                    case CardFunctions.Damage:
                        //Targeting core
                        if (cardEvent.targetType == TargetType.PlayerCore)
                        {
                            cardScore += coreDamagePoints * cardEvent.value;
                            break;
                        }
                        int partHealth = currPlayerIteration.bodyParts[(ConversionUtils.ConvertTargetToBodyPart(cardEvent.targetType))].MyCurrentHealth;
                        //Doesn't destroy or do core damage
                        if (partHealth > cardEvent.value)
                        {
                            cardScore += cardEvent.value;
                        }
                        //Does enough to destroy part
                        else if (partHealth == cardEvent.value)
                        {
                            cardScore += cardEvent.value * damagePoints;
                            cardScore += destroyPartPoints;
                        }
                        //Destroys and does damage to core
                        else if (partHealth > 0)
                        {
                            cardScore += partHealth;
                            cardScore += destroyPartPoints;
                            cardScore += (cardEvent.value - partHealth) * coreDamagePoints;
                        }
                        //Direct core damage
                        else
                        {
                            cardScore += coreDamagePoints * cardEvent.value;
                        }
                        break;
                    //Calculate score healing
                    case CardFunctions.Heal:
                        if (currEnemyIteration.currentCoreHealth + cardEvent.value <= currEnemyIteration.maxCoreHealth) cardScore += cardEvent.value * healPoints;
                        else cardScore += healPoints * (currEnemyIteration.maxCoreHealth - currEnemyIteration.currentCoreHealth);
                        break;
                    //Just add energy to the pool
                    case CardFunctions.GainEnergy:
                        currEnemyIteration.currentEnergy += cardEvent.value;
                        break;
                }
            }

            return cardScore;
        }

        private List<CardInstance> DetermineTurn(List<CardInstance> cards, int index, int score)
        {
            if (cards.Count == 0)
            {
                Debug.Log("No cards to play");
                return null;
            }
            else if (cards.Count == 1)
            {

            }
            else if (index == 0)
            {
                return PickBestCards();
            }
            else
            {

            }


            return cards;
        }


    }
}

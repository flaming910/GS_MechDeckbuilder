using MDB.Cards;
using MDB.Characters;
using MDB.Managers;
using UnityEngine;

namespace MDB.Controllers
{
    public class ControllerBase : MonoBehaviour
    {
        #pragma warning disable 0649
        [SerializeField] private bool isPlayer;
        /// <summary> Cards to draw every turn </summary>
        [SerializeField] private int cardsToDraw = 4;
        #pragma warning restore 0649

        /// <summary> reference to the UIManager instance </summary>
        private UIManager uiManager;

        #region Private Variables
        /// <summary> The character this controller holds </summary>
        private Character sourceCharacter;

        /// <summary> The source character's deck and discard pile </summary>
        private Deck deck;

        /// <summary> The source character's hand </summary>
        private Hand hand;

        /// <summary> Is it their turn</summary>
        private bool isMyTurn;
        #endregion



        public delegate bool OnCardClicked(CardInstance card);
        public static OnCardClicked onCardClicked;

        #region GETTERS
        /// <summary> The character this controller holds </summary>
        public Character MySourceCharacter => sourceCharacter;

        /// <summary> The source character's deck and discard pile </summary>
        public Deck MyDeck => deck;

        /// <summary> The source character's hand </summary>
        public Hand MyHand => hand;

        public bool IsMyTurn
        {
            get => isMyTurn;
            set => isMyTurn = value;
        }

        public bool IsPlayer => isPlayer;
        #endregion



        // Start is called before the first frame update
        private void Awake()
        {
            sourceCharacter = GetComponent<Character>();
            uiManager = UIManager.Instance;
            hand = new Hand(
                isPlayer ? uiManager.PlayerHand : uiManager.EnemyHand,
                uiManager.CardPrefab,
                isPlayer);

            if (isPlayer)
            {
                deck = new Deck(CardDatabase.Instance.CurrentPlayerCardList.CardList.ToArray());
            }
            else
            {
                deck = new Deck(CardDatabase.Instance.CommonEnemyCards.CardList.ToArray());
            }

            onCardClicked += TryPlayCard;

            sourceCharacter.SetUpVisuals(uiManager.HealthBarPrefab, isPlayer ? uiManager.PlayerHealthBars : uiManager.EnemyHealthBars);
        }

        public void UpdateDeck()
        {
            deck = new Deck(CardDatabase.Instance.CurrentPlayerCardList.CardList.ToArray());
        }

        /// <summary>
        /// Try to play a card
        /// </summary>
        /// <param name="card">Card to play</param>
        /// <returns>True if card gets played, false if it can't be</returns>
        public bool TryPlayCard(CardInstance card)
        {
            if (!isMyTurn) return false;
            var cardInfo = card.CardInfo;
            if (cardInfo.CardCanBePlayed(MySourceCharacter))
            {
                cardInfo.PlayCard(MySourceCharacter);
                deck.SendToDiscard(cardInfo);
                hand.RemoveCard(card);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Discard the characters hand and draw back up to the specified amount based on cardsToDraw
        /// </summary>
        public void DiscardAndDraw()
        {
            foreach (var card in hand.HandCards)
            {
                deck.SendToDiscard(card.CardInfo);
            }
            hand.EmptyHand();

            hand.AddCard(deck.Draw(cardsToDraw), this);
        }

    }

}


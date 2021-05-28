using MDB.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace MDB.UI
{
    /// <summary> This displays cards in player's deck. Should be reusable to show Full deck, Current deck, and Discard pile </summary>
    public class DeckViewer : MonoBehaviour
    {
        private static DeckViewer instance;

        public static DeckViewer Instance
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

        /// <summary> The actual object that will hold all viewed cards </summary>
        [SerializeField]
        private Transform cardHolder;

        /// <summary> The prefab of cards t </summary>
        [SerializeField]
        private GameObject cardPreviewPrefab;

        /// <summary> The cards that have already been created. To be reused whenever viewer is opened </summary>
        private List<GameObject> cardPool;

        /// <summary> The canvas group of the deck viewer </summary>
        [SerializeField]
        private CanvasGroup deckViewerGroup;

        private float cardSize;

        /// <summary> Start is called before the first frame update </summary>
        private void Start()
        {
            cardPool = new List<GameObject>();
            cardSize = cardPreviewPrefab.GetComponent<RectTransform>().sizeDelta.y * cardPreviewPrefab.GetComponent<RectTransform>().localScale.y + cardHolder.GetComponent<GridLayoutGroup>().cellSize.y;
        }

        /// <summary> View select deck. Go through card pool and set the cards to the cards of the deck in random order (account for rarity).
        /// If it is the first time, Instantiate a whole new pool.
        /// If there are more cards in deck than in pool, Instantiate the extras.
        /// If there are less cards in deck than in pool, just hide the extras.</summary>
        public void ToggleShowDeck(string deck)
        {
            if (deckViewerGroup.alpha == 0)
            {
                CardBase[] viewedCards = new CardBase[0];

                switch (deck) // Set which deck shall be viewed
                {
                    case "Full Deck":
                        viewedCards = CardDatabase.Instance.CurrentPlayerCardList.CardList.ToArray();
                        break;
                    case "Draw Pile":
                        viewedCards = Managers.GameManager.Instance.PlayerController.MyDeck.GetDeck();
                        break;
                    case "Discard Pile":
                        viewedCards = Managers.GameManager.Instance.PlayerController.MyDeck.GetDiscard();
                        break;
                }

                if (viewedCards.Length > cardPool.Count) // If there are more cards in the viewed deck than the card pool, make more cards
                {
                    for (int i = cardPool.Count; i < viewedCards.Length; i++)
                    {
                        GameObject go = Instantiate(cardPreviewPrefab, Vector3.zero, Quaternion.identity, cardHolder);
                        cardPool.Add(go);
                    }
                }
                else if(viewedCards.Length < cardPool.Count) // If there are less cards in the viewed deck than the card pool, hide excess cards
                {
                    for (int i = cardPool.Count - 1; i >= viewedCards.Length; i--)
                    {
                        cardPool[i].SetActive(false);
                    }
                }

                for (int i = 0; i < viewedCards.Length; i++)
                {
                    cardPool[i].GetComponent<CardPreview>().SetUpCard(viewedCards[i], MDB.Managers.GameManager.Instance.PlayerController, true);
                    cardPool[i].SetActive(true); // Make sure all viewed cards are showing
                }

                deckViewerGroup.alpha = 1;
                deckViewerGroup.blocksRaycasts = true;
                cardHolder.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    cardHolder.GetComponent<RectTransform>().sizeDelta.x,
                    Mathf.Ceil( cardHolder.childCount / 4.0f) * cardSize);
            }
            else
            {
                deckViewerGroup.alpha = 0;
                deckViewerGroup.blocksRaycasts = false;
            }
        }
    }
}
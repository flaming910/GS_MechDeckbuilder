using MDB.Interfaces;
using MDB.Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MDB.Cards
{
    /// <summary> The actual card object in the game. This sets up the visuals for name, description, etc... </summary>
    public class CardInstance : MonoBehaviour, IClickable
    {
        #region Serialized Variables

        #pragma warning disable 0649
        /// <summary> Reference to the card title </summary>
        [SerializeField]
        private TextMeshProUGUI title;

        /// <summary> Reference to the card art </summary>
        [SerializeField]
        private Image art;

        /// <summary> Reference to the card cost </summary>
        [SerializeField]
        private TextMeshProUGUI cost;

        /// <summary> Reference to the card description </summary>
        [SerializeField]
        private TextMeshProUGUI description;

        /// <summary> Reference to the card source </summary>
        [SerializeField]
        private TextMeshProUGUI source;
#pragma warning restore 0649
        #endregion

        /// <summary> The card. This contains name, cost, event, etc... </summary>
        private CardBase cardInfo;

        /// <summary> Which controller this card belongs to </summary>
        private ControllerBase controller;

        /// <summary> Whether this card is for the player </summary>
        private bool isPlayerCard;

        #region Getters
        /// <summary> The card. This contains name, cost, event, etc... </summary>
        public CardBase CardInfo => cardInfo;
        #endregion

        /// <summary>
        /// Set up the physical card details
        /// </summary>
        /// <param name="c"> The card information </param>
        /// <param name="control"> The controller of this card </param>
        public void SetUpCard(CardBase c, ControllerBase control, bool isPlayerCard)
        {
            cardInfo = c;

            title.text = cardInfo.CardName;

            art.sprite = c.CardArt;

            cost.text = cardInfo.CardCost.ToString();

            description.text = cardInfo.CardText;

            source.text = cardInfo.Source.ToString();

            controller = control;

            this.isPlayerCard = isPlayerCard;
        }

        /// <summary> When mouse gets over this card, scale it up. Also, move it up a bit to see bottom </summary>
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (!isPlayerCard) return;
            transform.localScale = new Vector3(transform.localScale.x * 1.4f, transform.localScale.y * 1.4f, 1);

            transform.position = new Vector3(transform.position.x, transform.position.y + 1.2f, 1);
        }

        /// <summary> When mouse moves away from this card, scale it back down. Also move it back down </summary>
        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (!isPlayerCard) return;
            transform.localScale = new Vector3(transform.localScale.x * 5/7, transform.localScale.y * 5/7, 1);

            transform.position = new Vector3(transform.position.x, transform.position.y - 1.2f, 1);
        }

        /// <summary> When mouse clicks this card, play it </summary>
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (!isPlayerCard) return;
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                controller.TryPlayCard(this);
            }
        }
    }
}

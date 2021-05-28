using MDB.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MDB.Cards
{
    /// <summary> The physcial card shown in the rewards tab. </summary>
    public class CardReward : CardInstance
    {
        /// <summary> When mouse gets over this card, scale it up. </summary>
        public override void OnPointerEnter(PointerEventData eventData)
        {
            transform.localScale = new Vector3(transform.localScale.x * 1.4f, transform.localScale.y * 1.4f, 1);
        }

        /// <summary> When mouse moves away from this card, scale it back down. </summary>
        public override void OnPointerExit(PointerEventData eventData)
        {
            transform.localScale = new Vector3(transform.localScale.x * 5 / 7, transform.localScale.y * 5 / 7, 1);
        }

        /// <summary> When mouse clicks this card, add it to player's deck and close rewards tab. </summary>
        public override void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                CardDatabase.Instance.CurrentPlayerCardList.AddCardToList(this.CardInfo);
                RewardsPanel.Instance.CloseRewards();
            }
        }
    }
}

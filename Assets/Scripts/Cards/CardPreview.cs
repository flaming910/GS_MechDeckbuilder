using MDB.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MDB.Cards
{
    /// <summary> The physical card shown in the deck viewer. /summary>
    public class CardPreview : CardInstance
    {
        /// <summary> When mouse gets over this card, scale it up. </summary>
        public override void OnPointerEnter(PointerEventData eventData)
        {
            transform.localScale = new Vector3(transform.localScale.x * 1.1f, transform.localScale.y * 1.1f, 1);
        }

        /// <summary> When mouse moves away from this card, scale it back down. </summary>
        public override void OnPointerExit(PointerEventData eventData)
        {
            transform.localScale = new Vector3(transform.localScale.x * 10 / 11, transform.localScale.y * 10 / 11, 1);
        }

        /// <summary> When mouse clicks this card, do nothing. Maybe zoom in, focus on this single card?? </summary>
        public override void OnPointerClick(PointerEventData eventData)
        {

        }
    }
}

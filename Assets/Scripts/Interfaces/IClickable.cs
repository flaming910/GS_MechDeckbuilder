using UnityEngine.EventSystems;

namespace MDB.Interfaces
{
    /// <summary> Stuff that can be clicked. Clicking them does something, but also hovering over them does something :) </summary>
    public interface IClickable: IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
    }
}

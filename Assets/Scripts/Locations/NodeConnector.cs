using UnityEngine;

namespace MDB.Locations
{
    public class NodeConnector : MonoBehaviour
    {
        /// <summary>
        /// Take two transforms and draw a line between them
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        public void CreateConnection(RectTransform parent, RectTransform child)
        {
            var lineRenderer = GetComponent<LineRenderer>();
            // ReSharper disable twice Unity.InefficientPropertyAccess
            lineRenderer.useWorldSpace = true;
            var parentPos = Camera.main.ScreenToWorldPoint(new Vector3(parent.position.x, parent.position.y, 0));
            parentPos.z = 0;
            var childPos = Camera.main.ScreenToWorldPoint(new Vector3(child.position.x, child.position.y, 0));
            childPos.z = 0;
            lineRenderer.SetPosition(0, parentPos);
            lineRenderer.SetPosition(1, childPos);
        }

    }

}


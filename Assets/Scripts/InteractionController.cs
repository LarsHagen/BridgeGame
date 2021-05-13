using UnityEngine;
using UnityEngine.EventSystems;

namespace BridgeGame
{
    public class InteractionController : MonoBehaviour, IDragHandler, IBeginDragHandler
    {
        public enum Tool { Move, Delete, Build, ChangeType}
        public Tool selectedTool;
        public ConnectionType selectedType;
        public bool gridOn;

        private Vector3 worldLast;

        public void OnDrag(PointerEventData eventData)
        {
            Camera.main.transform.position -= eventData.pointerCurrentRaycast.worldPosition - worldLast;
            worldLast = eventData.pointerCurrentRaycast.worldPosition;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            worldLast = eventData.pointerCurrentRaycast.worldPosition;
        }
    }
}

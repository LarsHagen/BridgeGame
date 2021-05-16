using UnityEngine;
using UnityEngine.EventSystems;

namespace BridgeGame
{
    public class PointView : MonoBehaviour, IDragHandler, IPointerClickHandler, IEndDragHandler
    {
        public IPoint point;
        private InteractionController interactionController;
        public Bridge bridge;

        private void Awake()
        {
            interactionController = FindObjectOfType<InteractionController>();
        }

        private void LateUpdate()
        {
            if (bridge.SimulationRunning)
                transform.position = point.Rigidbody2D.position;
            else
                transform.position = point.StartPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            interactionController.OnDragPointView(this, eventData);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            interactionController.OnClickPointView(this, eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            interactionController.OnEndDragPointView(this, eventData);
        }
    }
}

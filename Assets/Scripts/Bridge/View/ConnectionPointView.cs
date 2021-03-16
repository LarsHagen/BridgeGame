using UnityEngine;
using UnityEngine.EventSystems;

namespace BridgeGame
{
    public class ConnectionPointView : MonoBehaviour, IDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IEndDragHandler
    {
        public IPoint point;
        private InteractionController interactionController;
        private static ConnectionPointView buildEnd;
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
            if (interactionController.selectedTool == InteractionController.Tool.Move)
            {
                Plane plane = new Plane(Vector3.forward, Vector3.zero);
                var ray = Camera.main.ScreenPointToRay(eventData.position);
                if (plane.Raycast(ray, out float enter))
                {
                    Vector3 hitPoint = ray.GetPoint(enter);
                    //transform.position = hitPoint;
                    point.Move(hitPoint);

                    Debug.Log("MOVE!");
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (interactionController.selectedTool == InteractionController.Tool.Delete)
                bridge.RemovePoint(point);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (buildEnd == this)
                buildEnd = null;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            buildEnd = this;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (buildEnd != null)
                bridge.AddConnection(point, buildEnd.point, interactionController.selectedType);
            else
            {
                Plane plane = new Plane(Vector3.forward, Vector3.zero);
                var ray = Camera.main.ScreenPointToRay(eventData.position);
                if (plane.Raycast(ray, out float enter))
                {
                    Vector3 hitPoint = ray.GetPoint(enter);
                    var newPoint = bridge.AddPoint(hitPoint);
                    bridge.AddConnection(point, newPoint, interactionController.selectedType);

                    Debug.Log("BUILD!");
                }
            }
        }
    }
}

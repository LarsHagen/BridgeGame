using UnityEngine;
using UnityEngine.EventSystems;

namespace BridgeGame
{
    public class PointView : MonoBehaviour, IDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IEndDragHandler
    {
        public IPoint point;
        private InteractionController interactionController;
        private static PointView buildEnd;
        public Bridge bridge;
        private ConnectionView ghost;

        private void Awake()
        {
            ghost = GameObject.Find("ConnectionView_GHOST").GetComponent<ConnectionView>();
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
                    if (interactionController.GridOn)
                        hitPoint = SnapToGrid(hitPoint);

                    point.Move(hitPoint);
                }
            }

            if (interactionController.selectedTool == InteractionController.Tool.Build)
            {
                Plane plane = new Plane(Vector3.forward, Vector3.zero);
                var ray = Camera.main.ScreenPointToRay(eventData.position);
                if (plane.Raycast(ray, out float enter))
                {
                    Vector3 hitPoint = ray.GetPoint(enter);
                    var endPoint = interactionController.GridOn ? SnapToGrid(hitPoint) : hitPoint;
                    var startPoint = point.StartPosition;
                    var stress = Vector2.Distance(startPoint, endPoint) > 2.1f ? 1 : 0;
                    ghost.Setposition(startPoint, endPoint, stress);
                }
            }
        }

        private Vector3 SnapToGrid(Vector3 point)
        {
            float gridSize = 0.25f;
            
            point *= (1f / gridSize);
            point = new Vector3(Mathf.Round(point.x), Mathf.Round(point.y), Mathf.Round(point.z));
            point *= gridSize;

            return point;
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
            ghost.transform.position = new Vector3(0, 0, -1000);
            if (interactionController.selectedTool == InteractionController.Tool.Build)
            {
                if (buildEnd != null)
                {
                    bridge.AddConnection(point, buildEnd.point, interactionController.selectedType);
                }
                else
                {
                    Plane plane = new Plane(Vector3.forward, Vector3.zero);
                    var ray = Camera.main.ScreenPointToRay(eventData.position);
                    if (plane.Raycast(ray, out float enter))
                    {
                        Vector3 hitPoint = ray.GetPoint(enter);
                        
                        if (interactionController.GridOn)
                            hitPoint = SnapToGrid(hitPoint);

                        if (Vector2.Distance(point.StartPosition, hitPoint) > 2.1f)
                            return;

                        var newPoint = bridge.AddPoint(hitPoint);
                        bridge.AddConnection(point, newPoint, interactionController.selectedType);
                    }
                }
            }
        }
    }
}

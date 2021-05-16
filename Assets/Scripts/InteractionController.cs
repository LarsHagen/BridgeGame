using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BridgeGame
{
    public class InteractionController : MonoBehaviour, IDragHandler
    {
        public enum Tool { Move, Delete, Build, ChangeType}
        public Tool selectedTool;
        public ConnectionType selectedType;
        public bool GridOn { get; private set; }

        private MeshRenderer grid;

        private ConnectionView ghostConnectionView;

        private void Awake()
        {
            grid = GetComponentInChildren<MeshRenderer>();
            ghostConnectionView = GameObject.Find("ConnectionView_GHOST").GetComponent<ConnectionView>();
        }

        public void ShowGrid()
        {
            GridOn = true;
            grid.enabled = true;
        }

        public void HideGrid()
        {
            GridOn = false;
            grid.enabled = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            //Move camera
            var last = Camera.main.ScreenToWorldPoint(eventData.position - eventData.delta);
            var current = Camera.main.ScreenToWorldPoint(eventData.position);
            Camera.main.transform.position += last - current;
        }

        public void OnDragPointView(PointView pointView, PointerEventData eventData)
        {
            var point = pointView.point;
            var bridge = pointView.bridge;

            var position = GetPointOnBuildPlane(eventData);

            if (!position.HasValue)
            {
                Debug.LogWarning("No point on build plane");
                return;
            }

            if (selectedTool == Tool.Move)
            {
                point.Move(position.Value);
            }

            if (selectedTool == Tool.Build)
            {
                var startPoint = point.StartPosition;
                var stress = Vector2.Distance(startPoint, position.Value) > bridge.maxLengths[selectedType] ? 1 : 0;
                ghostConnectionView.Setposition(startPoint, position.Value, stress);
            }
        }

        public void ClickedConnection(ConnectionView connectionView, PointerEventData eventData)
        {
            if (connectionView.ghost)
                return;

            var bridge = connectionView.bridge;
            var connection = connectionView.connection;

            if (selectedTool == Tool.Delete)
            {
                bridge.RemoveConnection(connection);
            }
            else if (selectedTool == Tool.Build)
            {
                var newPoint = bridge.AddPoint(eventData.pointerCurrentRaycast.worldPosition);
                bridge.AddConnection(newPoint, connection.B, connection.Type);
                bridge.AddConnection(newPoint, connection.A, connection.Type);
                bridge.RemoveConnection(connection);
            }
            else if (selectedTool == Tool.ChangeType)
            {
                bridge.RemoveConnection(connection);
                bridge.AddConnection(connection.A, connection.B, selectedType);
            }
        }

        public void OnEndDragPointView(PointView pointView, PointerEventData eventData)
        {
            var point = pointView.point;
            var bridge = pointView.bridge;

            ghostConnectionView.transform.position = new Vector3(0, 0, -1000);
            if (selectedTool == Tool.Build)
            {
                var hit = GetPointViewAt(eventData, pointView);
                if (hit != null)
                {
                    bridge.AddConnection(point, hit.point, selectedType);
                }
                else
                {
                    var position = GetPointOnBuildPlane(eventData);

                    if (position.HasValue)
                    {
                        if (Vector2.Distance(point.StartPosition, position.Value) > bridge.maxLengths[selectedType])
                            return;

                        var newPoint = bridge.AddPoint(position.Value);
                        bridge.AddConnection(point, newPoint, selectedType);
                    }
                }
            }
        }

        public void OnClickPointView(PointView pointView, PointerEventData eventData)
        {
            if (selectedTool == Tool.Delete)
                pointView.bridge.RemovePoint(pointView.point);
        }

        private PointView GetPointViewAt(PointerEventData eventData, PointView ignore)
        {
            var position = GetPointOnBuildPlane(eventData);
            if (!position.HasValue)
                return null;

            foreach (var point in FindObjectsOfType<PointView>())
            {
                if (point == ignore)
                    continue;

                if (Vector2.Distance(position.Value, point.transform.position) <= 0.1)
                    return point;
            }

            return null;
        }

        private Vector3 SnapToGrid(Vector3 point)
        {
            float gridSize = 0.25f;

            point *= (1f / gridSize);
            point = new Vector3(Mathf.Round(point.x), Mathf.Round(point.y), Mathf.Round(point.z));
            point *= gridSize;

            return point;
        }

        private Vector3? GetPointOnBuildPlane(PointerEventData eventData)
        {
            Plane plane = new Plane(Vector3.forward, Vector3.zero);
            var ray = Camera.main.ScreenPointToRay(eventData.position);
            if (plane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                if (GridOn)
                    hitPoint = SnapToGrid(hitPoint);

                return hitPoint;
            }
            return null;
        }
    }
}

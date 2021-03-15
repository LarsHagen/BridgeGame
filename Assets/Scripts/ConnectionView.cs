using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BridgeGame
{
    public class ConnectionView : MonoBehaviour, IPointerClickHandler
    {
        public Connection connection;
        private InteractionController interactionController;

        public EdgeCollider2D edgeCollider;

        public Color colorDefault;
        public Color colorRoad;

        private MeshRenderer meshRenderer;

        private void Awake()
        {
            interactionController = FindObjectOfType<InteractionController>();
            meshRenderer = GetComponent<MeshRenderer>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (interactionController.selectedTool == InteractionController.Tool.Delete)
            {
                connection.bridge.RemoveConnection(connection);
            }
            else if (interactionController.selectedTool == InteractionController.Tool.Build)
            {
                var newPoint = connection.bridge.AddPoint(eventData.pointerCurrentRaycast.worldPosition);
                connection.bridge.AddConnection(newPoint, connection.b, interactionController.selectedType);
                connection.b = newPoint;
            }
            else if (interactionController.selectedTool == InteractionController.Tool.ChangeType)
            {
                connection.isRoad = !connection.isRoad;
            }
        }

        private void LateUpdate()
        {
            transform.localScale = new Vector3(Vector2.Distance(connection.a.physics.position, connection.b.physics.position), 0.1f, 0.1f);
            transform.position = Vector2.Lerp(connection.a.physics.position, connection.b.physics.position, 0.5f);
            //transform.right = transform.position - (Vector3)connection.b.physics.position;
            Vector3 dir = connection.b.physics.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}

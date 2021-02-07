using UnityEngine;
using UnityEngine.EventSystems;

namespace BridgeGame
{
    public class ConnectionView : MonoBehaviour, IPointerClickHandler
    {
        public Connection connection;
        private InteractionController interactionController;

        private void Awake()
        {
            interactionController = FindObjectOfType<InteractionController>();
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
                connection.bridge.AddConnection(newPoint, connection.b);
                connection.b = newPoint;
            }
        }

        private void Update()
        {
            transform.position = Vector3.Lerp(connection.a.position, connection.b.position, 0.5f);
            transform.LookAt(connection.b.position);
            transform.localScale = new Vector3(0.1f, 0.1f, connection.length);
        }
    }
}

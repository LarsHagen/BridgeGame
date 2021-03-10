using UnityEngine;
using UnityEngine.EventSystems;

namespace BridgeGame
{
    public class ConnectionView : MonoBehaviour, IPointerClickHandler
    {
        public Connection connection;
        private InteractionController interactionController;
        public Collider2D roadCollider;

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
                connection.bridge.AddConnection(newPoint, connection.b);
                connection.b = newPoint;
            }
            else if (interactionController.selectedTool == InteractionController.Tool.ChangeType)
            {
                connection.isRoad = !connection.isRoad;
            }
        }

        private void Update()
        {
            transform.position = Vector3.Lerp(connection.a.position, connection.b.position, 0.5f);
            transform.LookAt(connection.b.position, new Vector3(0,0,1));
            transform.localScale = new Vector3(0.1f, 0.1f, connection.length);

            roadCollider.gameObject.SetActive(connection.isRoad);
            meshRenderer.material.SetColor("ColorMain", connection.isRoad ? colorRoad : colorDefault);

            if (connection.bridge.SimulationRunning)
                meshRenderer.material.SetFloat("Stress", Mathf.Abs(connection.CalculateForce()) / connection.maxStress);
            else
                meshRenderer.material.SetFloat("Stress", 0);

            meshRenderer.enabled = !connection.broken;
        }
    }
}

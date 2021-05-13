using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BridgeGame
{
    public class ConnectionView : MonoBehaviour, IPointerClickHandler
    {
        public IConnection connection;
        private InteractionController interactionController;

        public bool ghost;

        public Color colorGhost;
        public Color colorDefault;
        public Color colorRoad;

        private List<MeshRenderer> meshRenderers;
        public MeshRenderer roadRenderer;

        public Bridge bridge;

        private void Awake()
        {
            interactionController = FindObjectOfType<InteractionController>();
            meshRenderers = new List<MeshRenderer>(GetComponentsInChildren<MeshRenderer>());
        }

        private void Start()
        {
            if (ghost)
            {
                roadRenderer.enabled = false;
                meshRenderers.ForEach(mr => mr.material.SetColor("ColorMain", colorGhost));
            }
            else
            {
                roadRenderer.enabled = connection.Type == ConnectionType.Road;
                var color = connection.Type == ConnectionType.Road ? colorRoad : colorDefault;
                meshRenderers.ForEach(mr => mr.material.SetColor("ColorMain", color));
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (ghost)
                return;

            if (interactionController.selectedTool == InteractionController.Tool.Delete)
            {
                bridge.RemoveConnection(connection);
            }
            else if (interactionController.selectedTool == InteractionController.Tool.Build)
            {
                var newPoint = bridge.AddPoint(eventData.pointerCurrentRaycast.worldPosition);
                bridge.AddConnection(newPoint, connection.B, connection.Type);
                bridge.AddConnection(newPoint, connection.A, connection.Type);
                bridge.RemoveConnection(connection);
            }
            else if (interactionController.selectedTool == InteractionController.Tool.ChangeType)
            {
                bridge.RemoveConnection(connection);
                bridge.AddConnection(connection.A, connection.B, interactionController.selectedType);
            }
        }

        private void LateUpdate()
        {
            if (ghost)
                return;

            var visible = !bridge.SimulationRunning || !connection.Broken;
            meshRenderers.ForEach(mr => mr.enabled = visible);
            roadRenderer.enabled = visible && connection.Type == ConnectionType.Road;

            Vector2 posA = connection.A.Rigidbody2D.position;
            Vector2 posB = connection.B.Rigidbody2D.position;

            var stress = connection.Stress();
            if (!bridge.SimulationRunning)
                stress = Vector2.Distance(posA, posB) <= connection.MaxLength ? 0 : 1;

            Setposition(posA, posB, stress);
        }

        public void Setposition(Vector2 posA, Vector2 posB, float stress)
        {
            transform.localScale = new Vector3(Vector2.Distance(posA, posB), 0.1f, 0.1f);
            transform.position = Vector2.Lerp(posA, posB, 0.5f);
            Vector3 dir = (Vector3)posB - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            meshRenderers.ForEach(mr => mr.material.SetFloat("Stress", stress));
        }
    }
}

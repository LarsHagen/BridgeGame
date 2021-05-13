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

        private void Awake()
        {
            grid = GetComponentInChildren<MeshRenderer>();
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
            var last = Camera.main.ScreenToWorldPoint(eventData.position - eventData.delta);
            var current = Camera.main.ScreenToWorldPoint(eventData.position);
            Camera.main.transform.position += last - current;
        }
    }
}

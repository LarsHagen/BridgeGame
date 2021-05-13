using UnityEngine;

namespace BridgeGame
{
    public class InteractionController : MonoBehaviour
    {
        public enum Tool { Move, Delete, Build, ChangeType}
        public Tool selectedTool;
        public ConnectionType selectedType;
        public bool gridOn;
    }
}

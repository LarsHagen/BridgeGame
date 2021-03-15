using UnityEngine;

namespace BridgeGame
{
    public class InteractionController : MonoBehaviour
    {
        public enum Tool { Move, Delete, Build, ChangeType}
        public Tool selectedTool;

        public enum ConnectionType { Steel, Road }
        public ConnectionType selectedType;
    }
}

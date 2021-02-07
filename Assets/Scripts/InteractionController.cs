using UnityEngine;

namespace BridgeGame
{
    public class InteractionController : MonoBehaviour
    {
        public enum Tool { Move, Delete, Build}
        public Tool selectedTool;
    }
}

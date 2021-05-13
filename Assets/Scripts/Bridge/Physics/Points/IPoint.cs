using UnityEngine;

namespace BridgeGame
{
    public interface IPoint
    {
        Vector2 StartPosition { get; }
        Rigidbody2D Rigidbody2D { get; }

        void Setup(Vector2 position, Bridge bridge);
        void StartSimulation();
        void StopSimulation();
        void Remove();
        void DrawDebug();
        void Move(Vector2 newPosition);
    }
}

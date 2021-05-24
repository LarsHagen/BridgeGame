using System.Collections.Generic;
using UnityEngine;

namespace BridgeGame
{
    public class PointDefault : IPoint
    {
        public Vector2 StartPosition { get; private set; }
        private Bridge bridge;

        public Rigidbody2D Rigidbody2D { get; private set; }

        public void StartSimulation()
        {
            Rigidbody2D.velocity = Vector2.zero;

            float weight = 0;
            foreach (var connection in bridge.GetConnectionsAtPoint(this))
            {
                weight += connection.Weight() / 2f;
            }
            Rigidbody2D.mass = weight;
        }

        public void StopSimulation()
        {
            Rigidbody2D.transform.position = StartPosition;
            Rigidbody2D.velocity = Vector2.zero;
        }

        public virtual void DrawDebug()
        {
            Debug.DrawLine(Rigidbody2D.position + new Vector2(-0.1f, -0.1f), Rigidbody2D.position + new Vector2(0.1f, 0.1f), Color.white);
            Debug.DrawLine(Rigidbody2D.position + new Vector2(-0.1f, 0.1f), Rigidbody2D.position + new Vector2(0.1f, -0.1f), Color.white);
        }

        public virtual void Setup(Vector2 position, Bridge bridge)
        {
            this.StartPosition = position;
            this.bridge = bridge;
            Rigidbody2D = new GameObject("Point").AddComponent<Rigidbody2D>();
            Rigidbody2D.transform.SetParent(bridge.transform);
            Rigidbody2D.transform.position = position;
            Rigidbody2D.position = position;
            Rigidbody2D.gameObject.layer = LayerMask.NameToLayer("Bridge");
        }

        public virtual void Remove()
        {
            GameObject.Destroy(Rigidbody2D.gameObject);
        }

        public void Move(Vector2 newPosition)
        {
            this.StartPosition = newPosition;
            Rigidbody2D.transform.position = newPosition;
        }
    }
}
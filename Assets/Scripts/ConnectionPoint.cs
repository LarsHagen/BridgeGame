using System.Collections.Generic;
using UnityEngine;

namespace BridgeGame
{
    public class ConnectionPoint
    {
        public Vector2 preferedPosition = new Vector2();
        public Vector2 position = new Vector2();
        
        public bool locked;
        public Vector2 velocity = new Vector2();
        
        public Bridge bridge;
        public List<Connection> connections = new List<Connection>();
        public List<Vector2> addedForces = new List<Vector2>();

        public ConnectionPointView view;
        public Rigidbody2D physics;

        public ConnectionPoint(Vector2 position, bool locked, Bridge bridge)
        {
            this.position = position;
            this.locked = locked;
            this.bridge = bridge;

            physics = new GameObject("PhysicsPoint").AddComponent<Rigidbody2D>();
            physics.transform.position = position;

            if (locked)
                physics.constraints = RigidbodyConstraints2D.FreezePosition;
        }

        internal void StartSimulation()
        {
            physics.velocity = Vector2.zero;
        }

        internal void StopSimulation()
        {
            physics.transform.position = position;
            physics.velocity = Vector2.zero;
        }

        public void DrawDebug()
        {
            Debug.DrawLine(physics.position + new Vector2(-0.1f, -0.1f), physics.position + new Vector2(0.1f, 0.1f), locked ? Color.blue : Color.white);
            Debug.DrawLine(physics.position + new Vector2(-0.1f, 0.1f), physics.position + new Vector2(0.1f, -0.1f), locked ? Color.blue : Color.white);
        }
    }
}
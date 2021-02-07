using UnityEngine;

namespace BridgeGame
{
    public class ConnectionPoint
    {
        public Vector2 preferedPosition = new Vector2();
        public Vector2 position = new Vector2();
        public float gravity = -1f;
        public bool locked;
        public float damping = 10;
        public Vector2 velocity = new Vector2();
        public float mass = 1;
        public ConnectionPointView view;
        public Bridge bridge;

        public void ApplyForce(Vector2 connectionsForce)
        {
            if (locked)
                return;

            Vector2 dampingForce = velocity * damping;
            Vector2 force = connectionsForce + mass * new Vector2(0, gravity) - dampingForce;
            Vector2 acceleration = force / mass;
            velocity += acceleration * Time.fixedDeltaTime;
            position += velocity;
        }

        public void DrawDebug()
        {
            Debug.DrawLine(position + new Vector2(-0.1f, -0.1f), position + new Vector2(0.1f, 0.1f), locked ? Color.blue : Color.white);
            Debug.DrawLine(position + new Vector2(-0.1f, 0.1f), position + new Vector2(0.1f, -0.1f), locked ? Color.blue : Color.white);
        }
    }
}
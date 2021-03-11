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
        public ConnectionPointView view;
        public Bridge bridge;
        public List<Connection> connections = new List<Connection>();

        public Dictionary<TestSphere, float> testSphereCollisions = new Dictionary<TestSphere, float>(); 

        public void CalculateAndApplyForces()
        {
            if (locked)
                return;

            float calculatedMass = GetMass();

            var connectionsForce = Vector2.zero;
            foreach (var connection in connections)
            {
                Vector2 direction = (connection.a == this ? connection.b.position : connection.a.position) - position;
                direction.Normalize();
                var connectionForce = (connection.CalculateForce());
                connectionsForce += direction * connectionForce;
            }

            Vector2 gravity = calculatedMass * new Vector2(0, bridge.gravity);
            ApplyForce(connectionsForce + gravity, calculatedMass);

            Debug.DrawRay(position, gravity * 0.1f, Color.green);
            Debug.DrawRay(position, connectionsForce * 0.1f, Color.red);

            testSphereCollisions.Clear();
        }

        private float GetMass()
        {
            float calculatedMass = 0.1f;
            foreach (var connection in connections)
                if (!connection.broken)
                    calculatedMass += connection.GetWeight() / 2f;
            return calculatedMass;
        }

        private void ApplyForce(Vector2 force, float mass)
        {
            if (locked)
                return;

            Vector2 dampingForce = velocity * bridge.damping;
            force -= dampingForce;
            Vector2 acceleration = force / mass;
            velocity += acceleration * Time.fixedDeltaTime;
        }

        public void ApplyVelocity()
        {
            position += velocity;
        }

        public void DrawDebug()
        {
            Debug.DrawLine(position + new Vector2(-0.1f, -0.1f), position + new Vector2(0.1f, 0.1f), locked ? Color.blue : Color.white);
            Debug.DrawLine(position + new Vector2(-0.1f, 0.1f), position + new Vector2(0.1f, -0.1f), locked ? Color.blue : Color.white);
        }
    }
}
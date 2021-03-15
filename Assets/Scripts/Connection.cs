using Assets.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BridgeGame
{
    public class Connection
    {
        public ConnectionPoint a;
        public ConnectionPoint b;
        public float length;
        public float preferedLength;
        
        public Bridge bridge;
        public bool isRoad;
        public float normalizedMaxDeformation = 0.02f;
        public bool broken;

        public ConnectionView view;
        public SpringJoint2D physics;
        public RoadCollider roadCollider;

        public Connection(ConnectionPoint a, ConnectionPoint b, Bridge bridge, bool isRoad)
        {
            this.a = a;
            this.b = b;
            this.bridge = bridge;
            this.isRoad = isRoad;

            physics = a.physics.gameObject.AddComponent<SpringJoint2D>();
            physics.frequency = 10;
            physics.connectedBody = b.physics;
            physics.autoConfigureConnectedAnchor = false;
            physics.autoConfigureDistance = false;

            if (isRoad)
            {
                roadCollider = GameObject.Instantiate(Resources.Load<RoadCollider>("RoadCollider"));
                roadCollider.Setup(a.physics, b.physics);
            }
        }

        public void ShouldBreak()
        {
            if (NormalizedDeformation() > normalizedMaxDeformation)
            {
                physics.enabled = false;
                if (roadCollider != null)
                    roadCollider.gameObject.SetActive(false);
            }
        }

        public float GetWeight()
        {
            return preferedLength * 3;
        }

        public float GetDeformation()
        {
            length = Vector2.Distance(a.physics.position, b.physics.position);
            return (length - preferedLength);
        }

        public float NormalizedDeformation()
        {
            return Mathf.Abs(GetDeformation()) / preferedLength;
        }

        public float CalculateForce()
        {
            if (bridge.SimulationRunning && (NormalizedDeformation() > normalizedMaxDeformation || broken))
            {
                broken = true;
                return 0f;
            }

            return GetDeformation() * bridge.connectionForce;
        }

        internal void StartSimulation()
        {
            preferedLength = Vector2.Distance(a.position, b.position);
            physics.distance = preferedLength;
            physics.enabled = true;
            if (isRoad)
            {
                roadCollider.StartSimulation();
                roadCollider.gameObject.SetActive(true);
            }
        }

        internal void StopSimulation()
        {
            if (isRoad)
                roadCollider.StopSimulation();
        }

        public void DrawDebug()
        {
            Debug.DrawLine(a.physics.position, b.physics.position, Color.Lerp(Color.blue, Color.red, NormalizedDeformation() / normalizedMaxDeformation));
        }
    }
}
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
        public ConnectionView view;
        public Bridge bridge;
        public bool isRoad;
        public float normalizedMaxDeformation = 0.05f;
        public bool broken;

        public float addedWeight;

        public float GetWeight()
        {
            return preferedLength * 2;
        }

        public float GetDeformation()
        {
            length = Vector2.Distance(a.position, b.position);
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

        public void DrawDebug()
        {
            Debug.DrawLine(a.position, b.position, Color.Lerp(Color.blue, Color.red, NormalizedDeformation()));
        }
    }
}
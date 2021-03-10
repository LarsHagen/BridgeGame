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

        public void Evaluate(Dictionary<ConnectionPoint, Vector2> calculatedForces)
        {
            float force = CalculateForce();

            Vector2 direction = (b.position - a.position).normalized;

            Vector2 forceA = default;
            Vector2 forceB = default;

            if (a.locked)
            {
                forceB -= direction * force * 2;
            }
            else if (b.locked)
            {
                forceA += direction * force * 2;
            }
            else
            {
                forceA += direction * force;
                forceB -= direction * force;
            }

            if (calculatedForces.ContainsKey(a))
                calculatedForces[a] += forceA;
            else
                calculatedForces.Add(a, forceA);

            if (calculatedForces.ContainsKey(b))
                calculatedForces[b] += forceB;
            else
                calculatedForces.Add(b, forceB);
        }

        public float CalculateForce()
        {
            length = Vector2.Distance(a.position, b.position);
            return (length - preferedLength) * bridge.connectionForce;
        }

        public void DrawDebug()
        {
            Debug.DrawLine(a.position, b.position, Color.Lerp(Color.blue, Color.red, CalculateForce()));
        }
    }
}
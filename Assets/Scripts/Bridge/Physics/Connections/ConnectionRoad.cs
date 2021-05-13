using System.Collections.Generic;
using UnityEngine;

namespace BridgeGame
{
    public class ConnectionRoad : IConnection
    {
        public IPoint A { get; private set; }
        public IPoint B { get; private set; }
        public ConnectionType Type => ConnectionType.Road;
        public bool Broken { get; private set; }
        public float MaxLength => 2.1f;

        private float length;
        private float preferedLength;
        private Bridge bridge;

        private float normalizedMaxDeformation = 0.02f;
        private float stiffness = 17;
        private float dampingRatio = 0.5f;
        private float weightPerLength = 5;


        private Rigidbody2D road;
        private WheelJoint2D jointA;
        private WheelJoint2D jointB;
        private EdgeCollider2D edgeCollider;

        private float GetDeformation()
        {
            length = Vector2.Distance(A.Rigidbody2D.position, B.Rigidbody2D.position);
            return (length - preferedLength);
        }

        private float NormalizedDeformation()
        {
            return Mathf.Abs(GetDeformation()) / preferedLength;
        }

        public void StartSimulation()
        {
            Broken = false;
            preferedLength = Vector2.Distance(A.StartPosition, B.StartPosition);
            road.gameObject.SetActive(true);


            road.transform.position = Vector2.Lerp(A.StartPosition, B.StartPosition, 0.5f);
            Vector3 dir = (Vector3)A.StartPosition - road.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            road.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            edgeCollider.SetPoints(new List<Vector2>() { road.transform.InverseTransformPoint(A.StartPosition), road.transform.InverseTransformPoint(B.StartPosition) });

            jointA.connectedBody = A.Rigidbody2D;
            jointA.anchor = edgeCollider.points[0];
            jointB.connectedBody = B.Rigidbody2D;
            jointB.anchor = edgeCollider.points[1];
        }

        public void StopSimulation()
        {
            road.gameObject.SetActive(false);
        }

        public void DrawDebug()
        {
            Debug.DrawLine(A.Rigidbody2D.position, B.Rigidbody2D.position, Color.Lerp(Color.gray, Color.red, Stress()));
        }

        public void Setup(IPoint a, IPoint b, Bridge bridge)
        {
            A = a;
            B = b;
            this.bridge = bridge;

            road = new GameObject("Road").AddComponent<Rigidbody2D>();
            road.transform.SetParent(bridge.transform);
            road.gameObject.layer = LayerMask.NameToLayer("Bridge");

            edgeCollider = road.gameObject.AddComponent<EdgeCollider2D>();

            jointA = road.gameObject.AddComponent<WheelJoint2D>();
            jointB = road.gameObject.AddComponent<WheelJoint2D>();

            jointA.suspension = GetJointSuspensionSettings();
            jointB.suspension = GetJointSuspensionSettings();
        }

        private JointSuspension2D GetJointSuspensionSettings()
        {
            return new JointSuspension2D()
            {
                dampingRatio = dampingRatio,
                frequency = stiffness,
                angle = 0
            };
        }

        public void Remove()
        {
            GameObject.Destroy(road.gameObject);
        }

        public float Stress()
        {
            return NormalizedDeformation() / normalizedMaxDeformation;
        }

        public void Break()
        {
            road.gameObject.SetActive(false);
            Broken = true;
        }

        public float Weight()
        {
            return weightPerLength * Vector2.Distance(A.StartPosition, B.StartPosition);
        }

    }
}
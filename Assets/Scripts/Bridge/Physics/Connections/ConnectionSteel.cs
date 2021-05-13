using UnityEngine;

namespace BridgeGame
{
    public class ConnectionSteel : IConnection
    {
        public IPoint A { get; private set; }
        public IPoint B { get; private set; }
        public ConnectionType Type => ConnectionType.Steel;
        public bool Broken { get; private set; }
        public float MaxLength => 2;

        private float length;
        private float preferedLength;
        private Bridge bridge;

        private float normalizedMaxDeformation = 0.02f;
        private float stiffness = 15;
        private float dampingRatio = 0.5f;
        private float weightPerLength = 2f;

        private SpringJoint2D joint;

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
            joint.distance = preferedLength;
            joint.enabled = true;
        }

        public void StopSimulation()
        {
            joint.enabled = false;
        }

        public void DrawDebug()
        {
            Debug.DrawLine(A.Rigidbody2D.position, B.Rigidbody2D.position, Color.Lerp(Color.blue, Color.red, Stress()));
        }

        public void Setup(IPoint a, IPoint b, Bridge bridge)
        {
            A = a;
            B = b;
            this.bridge = bridge;

            joint = A.Rigidbody2D.gameObject.AddComponent<SpringJoint2D>();
            joint.frequency = stiffness;
            joint.dampingRatio = dampingRatio;
            joint.connectedBody = B.Rigidbody2D;
            joint.autoConfigureConnectedAnchor = false;
            joint.autoConfigureDistance = false;
        }

        public void Remove()
        {
            GameObject.Destroy(joint);
        }

        public float Stress()
        {
            return NormalizedDeformation() / normalizedMaxDeformation;
        }

        public void Break()
        {
            joint.enabled = false;
            Broken = true;
        }

        public float Weight()
        {
            return weightPerLength * Vector2.Distance(A.StartPosition, B.StartPosition);
        }
    }
}
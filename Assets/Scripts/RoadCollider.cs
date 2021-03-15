using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class RoadCollider : MonoBehaviour
    {
        public HingeJoint2D hingeJoint2D;
        public SliderJoint2D sliderJoint2D;
        public EdgeCollider2D edgeCollider2D;

        private Rigidbody2D pointA;
        private Rigidbody2D pointB;

        public void Setup(Rigidbody2D pointA, Rigidbody2D pointB)
        {
            this.pointA = pointA;
            this.pointB = pointB;

            SetValues();
        }

        private void SetValues()
        {
            transform.position = Vector2.Lerp(pointA.position, pointB.position, 0.5f);
            Vector3 dir = pointB.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            edgeCollider2D.SetPoints(new List<Vector2>() { transform.InverseTransformPoint(pointA.position), transform.InverseTransformPoint(pointB.position) });
            hingeJoint2D.connectedBody = pointA;
            hingeJoint2D.anchor = edgeCollider2D.points[0];
            sliderJoint2D.connectedBody = pointB;
            sliderJoint2D.anchor = edgeCollider2D.points[1];
        }

        public void StartSimulation()
        {
            SetValues();
        }

        public void StopSimulation()
        {
            SetValues();
        }
    }
}

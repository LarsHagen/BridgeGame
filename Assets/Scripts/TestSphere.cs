using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BridgeGame
{
    public class TestSphere : MonoBehaviour
    {
        public Bridge bridge;
        public Level level;

        public float weight;

        private void OnCollisionStay2D(Collision2D collision)
        {
            var connection = collision.collider.GetComponentInParent<ConnectionView>()?.connection;

            if (connection != null)
            {
                float distA = Vector2.Distance(collision.contacts[0].point, connection.a.position);
                float totalDist = Vector2.Distance(connection.a.position, connection.b.position);

                float normalA = distA / totalDist;

                float weightA = Mathf.Lerp(weight, 0, normalA);
                float weightB = weight - weightA;


                Debug.Log("A: " + weightA);
                Debug.Log("B: " + weightB);



                if (connection.a.testSphereCollisions.ContainsKey(this))
                    connection.a.testSphereCollisions[this] = weightA;
                else
                    connection.a.testSphereCollisions.Add(this, weightA);

                if (connection.b.testSphereCollisions.ContainsKey(this))
                    connection.b.testSphereCollisions[this] = weightB;
                else
                    connection.b.testSphereCollisions.Add(this, weightB);
            }
        }
    }
}
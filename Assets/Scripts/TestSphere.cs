using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BridgeGame
{
    public class TestSphere : MonoBehaviour
    {
        public Bridge bridge;
        public Level level;
        new public Rigidbody2D rigidbody2D;
        public float acceleration;
        public float topspeed;

        private void FixedUpdate()
        {
            if (bridge.SimulationRunning)
            {
                rigidbody2D.simulated = true;
                if (rigidbody2D.velocity.x < topspeed)
                    rigidbody2D.AddForce(new Vector2(acceleration, 0));
            }
            else
            {
                rigidbody2D.simulated = false;
                transform.position = level.start.position;
                rigidbody2D.velocity = Vector2.zero;
            }

            //DoGravityForce();
        }

    }
}
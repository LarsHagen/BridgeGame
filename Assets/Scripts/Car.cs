using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BridgeGame
{
    public class Car : MonoBehaviour
    {
        public Bridge bridge;
        public Level level;
        new public Rigidbody2D rigidbody2D;

        private void FixedUpdate()
        {
            if (bridge.SimulationRunning)
            {
                rigidbody2D.simulated = true;
            }
            else
            {
                rigidbody2D.simulated = false;
                transform.position = new Vector3(level.start.position.x, level.start.position.y, transform.position.z);
                transform.rotation = Quaternion.identity;
                rigidbody2D.velocity = Vector2.zero;
            }
        }

    }
}
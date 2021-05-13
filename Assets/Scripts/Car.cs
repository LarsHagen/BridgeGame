using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BridgeGame
{
    public class Car : MonoBehaviour
    {
        private GameController gameController;
        new public Rigidbody2D rigidbody2D;

        private void Awake()
        {
            gameController = FindObjectOfType<GameController>();
        }

        private void FixedUpdate()
        {

            if (gameController.SimulationRunning)
            {
                rigidbody2D.simulated = true;
            }
            else
            {
                rigidbody2D.simulated = false;
                transform.position = new Vector3(gameController.CurrentLevel.start.position.x, gameController.CurrentLevel.start.position.y, transform.position.z);
                transform.rotation = Quaternion.identity;
                rigidbody2D.velocity = Vector2.zero;
            }
        }

    }
}
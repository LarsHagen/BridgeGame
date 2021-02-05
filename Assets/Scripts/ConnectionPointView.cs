using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BridgeGame
{
    public class ConnectionPointView : MonoBehaviour
    {
        public ConnectionPoint point;

        private void Update()
        {
            transform.position = point.position;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace BridgeGame
{
    public class PointLocked : PointDefault
    {
        public override void Setup(Vector2 position, Bridge bridge)
        {
            base.Setup(position, bridge);
            Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition;
        }

        public override void DrawDebug()
        {
            Debug.DrawLine(Rigidbody2D.position + new Vector2(-0.1f, -0.1f), Rigidbody2D.position + new Vector2(0.1f, 0.1f), Color.blue);
            Debug.DrawLine(Rigidbody2D.position + new Vector2(-0.1f, 0.1f), Rigidbody2D.position + new Vector2(0.1f, -0.1f), Color.blue);
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    public static List<Connection> connections = new List<Connection>();


    private void FixedUpdate()
    {
        Dictionary<ConnectionPoint, Vector2> calculatedForces = new Dictionary<ConnectionPoint, Vector2>();
        connections.ForEach(c => c.Evaluate(calculatedForces));

        foreach (var calculatedForce in calculatedForces)
        {
            //calculatedForce.Key.transform.position += (Vector3)calculatedForce.Value;
            calculatedForce.Key.ApplyForce(calculatedForce.Value);
        }
    }

    private void Update()
    {
        connections.ForEach(c => c.DrawDebug());
    }
}

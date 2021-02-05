using System.Linq;
using UnityEngine;

public class ConnectionPoint : MonoBehaviour
{
    [SerializeField] private ConnectionPoint[] connections;
    [SerializeField] private float gravity = 1f;

    public bool locked;

    public float damping = 10;
    public Vector2 velocity = new Vector2();
    public float mass = 1;

    public void ApplyForce(Vector2 connectionsForce)
    {
        if (locked)
            return;

        Vector2 dampingForce = velocity * damping;
        Vector2 force = connectionsForce + mass * new Vector2(0, gravity) - dampingForce;
        Vector2 acceleration = force / mass;
        velocity += acceleration * Time.fixedDeltaTime;
        transform.position += (Vector3)velocity;
    }

    private void Awake()
    {
        foreach (var connection in connections)
        {
            var exsists = ConnectionManager.connections.Any(c => (c.a == this && c.b == connection) || (c.a == connection && c.b == this));
            if (exsists)
                continue;

            ConnectionManager.connections.Add(new Connection()
            {
                a = this,
                b = connection,
                preferedLength = Vector2.Distance(transform.position, connection.transform.position)
            });
        }
    }

}

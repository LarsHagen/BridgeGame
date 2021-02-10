using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BridgeGame
{
    public class Bridge : MonoBehaviour
    {
        public float connectionForce = 20f;
        public float gravity = -1f;
        public float damping = 10;

        public ConnectionPointView pointViewPrefab;
        public ConnectionView connectionViewPrefab;

        public static List<Connection> connections = new List<Connection>();
        public static List<ConnectionPoint> points = new List<ConnectionPoint>();

        public bool SimulationRunning { get; private set; }

        public void StartSimulation()
        {
            foreach (var connection in connections)
                connection.preferedLength = Vector2.Distance(connection.a.preferedPosition, connection.b.preferedPosition);

            SimulationRunning = true;
        }

        public void StopSimulation()
        {
            foreach (var point in points)
            {
                point.position = point.preferedPosition;
                point.velocity = Vector2.zero;
            }

            SimulationRunning = false;
        }

        private void Start()
        {
            TestPoints();
            TestConnection();
            StopSimulation();
        }

        public void RemovePoint(ConnectionPoint point)
        {
            foreach (var connection in GetConnectionsAtPoint(point))
                RemoveConnection(connection);

            Destroy(point.view.gameObject);
            points.Remove(point);
        }

        public IReadOnlyList<Connection> GetConnectionsAtPoint(ConnectionPoint point)
        {
            var list = new List<Connection>();
            foreach (var connection in connections)
            {
                if (connection.a == point || connection.b == point)
                    list.Add(connection);
            }

            return list;
        }

        public Connection GetConnectionBetweenPoints(ConnectionPoint pointA, ConnectionPoint pointB)
        {
            return GetConnectionsAtPoint(pointA).FirstOrDefault(c => c.a == pointB || c.b == pointB);
        }

        public void RemoveConnection(Connection connection)
        {
            Destroy(connection.view.gameObject);
            connections.Remove(connection);
        }

        public ConnectionPoint AddPoint(Vector2 position, bool locked = false)
        {
            var point = new ConnectionPoint()
            {
                locked = locked,
                preferedPosition = position,
                position = position,
                bridge = this
            };

            points.Add(point);
            var view = Instantiate(pointViewPrefab);
            view.point = point;
            point.view = view;

            return point;
        }

        public Connection AddConnection(ConnectionPoint pointA, ConnectionPoint pointB)
        {
            if (GetConnectionBetweenPoints(pointA, pointB) != null)
            {
                Debug.Log("Connection already exsists");
                return null;
            }

            var connection = new Connection()
            {
                a = pointA,
                b = pointB,
                preferedLength = Vector2.Distance(pointA.position, pointB.position),
                bridge = this
            };
            connections.Add(connection);

            var view = Instantiate(connectionViewPrefab);
            view.connection = connection;
            connection.view = view;

            return connection;
        }

        private void TestConnection()
        {
            AddConnection(points[0], points[1]);
            AddConnection(points[1], points[2]);
            AddConnection(points[2], points[3]);
        }

        private void TestPoints()
        {
            AddPoint(new Vector2(0, 0), true);
            AddPoint(new Vector2(1, 0));
            AddPoint(new Vector2(2, 0));
            AddPoint(new Vector2(3, 0), true);
        }

        private void FixedUpdate()
        {
            if (!SimulationRunning)
                return;

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

            points.ForEach(p => p.DrawDebug());
            connections.ForEach(c => c.DrawDebug());

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (SimulationRunning)
                    StopSimulation();
                else
                    StartSimulation();
            }
        }
    }
}
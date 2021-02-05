using System.Collections.Generic;
using UnityEngine;

namespace BridgeGame
{
    public class Bridge : MonoBehaviour
    {
        public ConnectionPointView pointViewPrefab;
        public ConnectionView connectionViewPrefab;

        public static List<Connection> connections = new List<Connection>();
        public static List<ConnectionPoint> points = new List<ConnectionPoint>();

        private void Start()
        {
            TestPoints();
            TestConnection();
        }

        public void AddPoint(Vector2 position, bool locked = false)
        {
            var point = new ConnectionPoint()
            {
                locked = locked,
                position = position
            };

            points.Add(point);
            var view = Instantiate(pointViewPrefab);
            view.point = point;
        }

        public void AddConnection(ConnectionPoint pointA, ConnectionPoint pointB)
        {
            var connection = new Connection()
            {
                a = pointA,
                b = pointB,
                preferedLength = Vector2.Distance(pointA.position, pointB.position)
            };
            connections.Add(connection);

            var view = Instantiate(connectionViewPrefab);
            view.connection = connection;
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
        }
    }
}
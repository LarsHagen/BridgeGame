using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static BridgeGame.InteractionController;

namespace BridgeGame
{
    public class Bridge : MonoBehaviour
    {
        public float connectionForce = 100f;
        public float gravity = -0.1f;
        public float damping = 1;

        public Level level;

        public ConnectionPointView pointViewPrefab;
        public ConnectionView connectionViewPrefab;

        public static List<Connection> connections = new List<Connection>();
        public static List<ConnectionPoint> points = new List<ConnectionPoint>();

        public bool SimulationRunning { get; private set; }

        public void StartSimulation()
        {
            foreach (var connection in connections)
                connection.StartSimulation();

            foreach (var point in points)
                point.StartSimulation();

            Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
            SimulationRunning = true;
        }

        public void StopSimulation()
        {
            Physics2D.simulationMode = SimulationMode2D.Script;

            foreach (var point in points)
                point.StopSimulation();

            foreach (var connection in connections)
                connection.StopSimulation();

            SimulationRunning = false;
        }

        private void Start()
        {
            SetupLevel();
            StopSimulation();
        }

        private void SetupLevel()
        {
            foreach (var point in level.attachmentPoints)
                AddPoint(point.transform.position, true);
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
            connection.a.connections.Remove(connection);
            connection.b.connections.Remove(connection);
            //Destroy(connection.view.gameObject);
            connections.Remove(connection);
        }

        public ConnectionPoint AddPoint(Vector2 position, bool locked = false)
        {
            var point = new ConnectionPoint(position, locked, this);
            points.Add(point);
            var view = Instantiate(pointViewPrefab);
            view.transform.position = position;
            view.point = point;
            point.view = view;

            return point;
        }

        public Connection AddConnection(ConnectionPoint pointA, ConnectionPoint pointB, ConnectionType type)
        {
            if (GetConnectionBetweenPoints(pointA, pointB) != null)
            {
                Debug.Log("Connection already exsists");
                return null;
            }

            var connection = new Connection(pointA, pointB, this, type == ConnectionType.Road);

            pointA.connections.Add(connection);
            pointB.connections.Add(connection);
            connections.Add(connection);

            var view = Instantiate(connectionViewPrefab);
            view.connection = connection;
            connection.view = view;

            return connection;
        }

        private void FixedUpdate()
        {
            connections.ForEach(c => c.ShouldBreak());
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
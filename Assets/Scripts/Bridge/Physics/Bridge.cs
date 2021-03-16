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
        //public ConnectionView connectionViewPrefab;

        public List<IConnection> connections = new List<IConnection>();
        public List<IPoint> points = new List<IPoint>();

        public Dictionary<IPoint, ConnectionPointView> pointViews = new Dictionary<IPoint, ConnectionPointView>();

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

        public void RemovePoint(IPoint point)
        {
            foreach (var connection in GetConnectionsAtPoint(point))
                RemoveConnection(connection);

            point.Remove();

            Destroy(pointViews[point].gameObject);
            pointViews.Remove(point);

            points.Remove(point);
        }

        public IReadOnlyList<IConnection> GetConnectionsAtPoint(IPoint point)
        {
            var list = new List<IConnection>();
            foreach (var connection in connections)
            {
                if (connection.A == point || connection.B == point)
                    list.Add(connection);
            }

            return list;
        }

        public IConnection GetConnectionBetweenPoints(IPoint pointA, IPoint pointB)
        {
            return GetConnectionsAtPoint(pointA).FirstOrDefault(c => c.A == pointB || c.B == pointB);
        }

        public void RemoveConnection(IConnection connection)
        {
            connections.Remove(connection);
        }

        public IPoint AddPoint(Vector2 position, bool locked = false)
        {
            IPoint point = locked ? new PointLocked() : new PointDefault();
            point.Setup(position, this);

            var view = Instantiate(pointViewPrefab);
            view.transform.position = position;
            view.point = point;
            view.bridge = this;

            pointViews.Add(point, view);
            points.Add(point);

            return point;
        }

        public IConnection AddConnection(IPoint pointA, IPoint pointB, ConnectionType type)
        {
            if (GetConnectionBetweenPoints(pointA, pointB) != null)
            {
                Debug.Log("Connection already exsists");
                return null;
            }

            IConnection connection;
            switch (type)
            {
                case ConnectionType.Road:
                    connection = new ConnectionRoad();
                    break;
                default:
                    connection = new ConnectionSteel();
                    break;
            }
            connection.Setup(pointA, pointB, this);
            connections.Add(connection);
            return connection;
        }

        private void FixedUpdate()
        {
            foreach (var connection in connections)
            {
                if (!connection.Broken && connection.Stress() > 1f)
                    connection.Break();
            }
        }

        private void Update()
        {
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
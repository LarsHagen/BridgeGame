using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BridgeGame
{
    public class Bridge : MonoBehaviour
    {
        public PointView pointViewPrefab;
        public ConnectionView connectionViewPrefab;

        public List<IConnection> connections = new List<IConnection>();
        public List<IPoint> points = new List<IPoint>();

        public Dictionary<IPoint, PointView> pointViews = new Dictionary<IPoint, PointView>();
        public Dictionary<IConnection, ConnectionView> connectionViews = new Dictionary<IConnection, ConnectionView>();

        public bool SimulationRunning { get; private set; }

        public void StartSimulation()
        {
            if (!IsValid())
            {
                Debug.Log("Invalid bridge design");
                return;
            }

            foreach (var connection in connections)
                connection.StartSimulation();

            foreach (var point in points)
                point.StartSimulation();

            Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
            SimulationRunning = true;
        }

        private bool IsValid()
        {
            foreach (var connection in connections)
                if (Vector2.Distance(connection.A.StartPosition, connection.B.StartPosition) > connection.MaxLength)
                    return false;

            return true;
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
            StopSimulation();
        }

        public void SetupLevel(Level level)
        {
            ClearAll();
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

        

        private void ClearAll()
        {
            foreach (var point in points.ToArray())
                RemovePoint(point);
            foreach (var connection in connections.ToArray())
                RemoveConnection(connection);
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
            Destroy(connectionViews[connection].gameObject);
            connectionViews.Remove(connection);
            connection.Remove();
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

            var view = Instantiate(connectionViewPrefab);
            view.connection= connection;
            view.bridge = this;

            connectionViews.Add(connection, view);

            return connection;
        }

        private void FixedUpdate()
        {
            if (SimulationRunning)
            {
                foreach (var connection in connections)
                {
                    if (!connection.Broken && connection.Stress() > 1f)
                        connection.Break();
                }
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
using UnityEngine;

namespace BridgeGame
{
    public class ConnectionView : MonoBehaviour
    {
        public Connection connection;

        private void Update()
        {
            transform.position = Vector3.Lerp(connection.a.position, connection.b.position, 0.5f);
            transform.LookAt(connection.b.position);
            transform.localScale = new Vector3(0.1f, 0.1f, connection.length);
        }
    }
}

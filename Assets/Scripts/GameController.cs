using UnityEngine;

namespace BridgeGame
{
    public class GameController : MonoBehaviour
    {
        private CameraController cameraController;
        private Bridge bridge;
        private InteractionController interactionController;

        private void Awake()
        {
            interactionController = FindObjectOfType<InteractionController>();
            cameraController = FindObjectOfType<CameraController>();
            bridge = FindObjectOfType<Bridge>();
        }

        public void StartSimulation()
        {
            interactionController.HideGrid();
            bridge.StartSimulation();
            cameraController.SwitchToPlayCam();
        }

        public void StopSimulation()
        {
            interactionController.ShowGrid();
            bridge.StopSimulation();
            cameraController.SwitchToBuildCam();
        }
    }
}

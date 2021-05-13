using UnityEngine;

namespace BridgeGame
{
    public class GameController : MonoBehaviour
    {
        private CameraController cameraController;
        private Bridge bridge;
        private InteractionController interactionController;

        [SerializeField] private Level[] levels;
        private int currentLevel;
        public Level CurrentLevel => levels[currentLevel];

        public bool SimulationRunning => bridge.SimulationRunning;

        private void Awake()
        {
            interactionController = FindObjectOfType<InteractionController>();
            cameraController = FindObjectOfType<CameraController>();
            bridge = FindObjectOfType<Bridge>();
        }

        private void Start()
        {
            ChangeLevel(0);
            StopSimulation();
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

        public void NextLevel()
        {
            StopSimulation();
            ChangeLevel(+1);
        }
        public void PreviousLevel()
        {
            StopSimulation();
            ChangeLevel(-1);
        }

        private void ChangeLevel(int direction)
        {
            CurrentLevel.gameObject.SetActive(false);
            currentLevel = (int)Mathf.Repeat(currentLevel + direction, levels.Length);
            CurrentLevel.gameObject.SetActive(true);
            bridge.SetupLevel(CurrentLevel);
        }
    }
}

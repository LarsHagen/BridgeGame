using UnityEngine;
using UnityEngine.UI;

namespace BridgeGame
{
    public class UI : MonoBehaviour
    {
        [Header("Internal refs")]
        [SerializeField] private Toggle moveToggle;
        [SerializeField] private Toggle deleteToggle;
        [SerializeField] private Toggle buildToggle;

        [SerializeField] private Button simulationButton;

        [Header("External refs")]
        [SerializeField] private InteractionController interactionController;
        [SerializeField] private Bridge bridge;

        private void Awake()
        {
            moveToggle.onValueChanged.AddListener(on => { if (on) interactionController.selectedTool = InteractionController.Tool.Move; });
            deleteToggle.onValueChanged.AddListener(on => { if (on) interactionController.selectedTool = InteractionController.Tool.Delete; });
            buildToggle.onValueChanged.AddListener(on => { if (on) interactionController.selectedTool = InteractionController.Tool.Build; });

            simulationButton.onClick.AddListener(StartStopSimulation);
        }

        private void StartStopSimulation()
        {
            if (bridge.SimulationRunning)
                bridge.StopSimulation();
            else
                bridge.StartSimulation();
        }
    }
}
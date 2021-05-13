using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

namespace BridgeGame
{
    public class UI : MonoBehaviour
    {
        [Header("Internal refs")]
        [SerializeField] private Toggle moveToggle;
        [SerializeField] private Toggle deleteToggle;
        [SerializeField] private Toggle buildToggle;
        [SerializeField] private Toggle changeToggle;
        [SerializeField] private Dropdown dropdown;

        [SerializeField] private Button simulationButton;
        [SerializeField] private Button nextLevel;
        [SerializeField] private Button previousLevel;

        [Header("External refs")]
        [SerializeField] private InteractionController interactionController;
        [SerializeField] private Bridge bridge;

        private GameController gameController;

        private void Awake()
        {
            gameController = FindObjectOfType<GameController>();

            moveToggle.onValueChanged.AddListener(on => { if (on) interactionController.selectedTool = InteractionController.Tool.Move; });
            deleteToggle.onValueChanged.AddListener(on => { if (on) interactionController.selectedTool = InteractionController.Tool.Delete; });
            buildToggle.onValueChanged.AddListener(on => { if (on) interactionController.selectedTool = InteractionController.Tool.Build; });
            changeToggle.onValueChanged.AddListener(on => { if (on) interactionController.selectedTool = InteractionController.Tool.ChangeType; });

            simulationButton.onClick.AddListener(StartStopSimulation);

            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
            foreach (var connectionType in (ConnectionType[]) Enum.GetValues(typeof(ConnectionType)))
            {
                options.Add(new Dropdown.OptionData(connectionType.ToString()));
            }
            dropdown.AddOptions(options);

            dropdown.onValueChanged.AddListener(selected => interactionController.selectedType = (ConnectionType)selected);

            nextLevel.onClick.AddListener(() => gameController.NextLevel());
            previousLevel.onClick.AddListener(() => gameController.PreviousLevel());
        }

        private void Update()
        {
            dropdown.gameObject.SetActive(buildToggle.isOn || changeToggle.isOn);
        }

        private void StartStopSimulation()
        {
            if (gameController.SimulationRunning)
                gameController.StopSimulation();
            else
                gameController.StartSimulation();
        }
    }
}
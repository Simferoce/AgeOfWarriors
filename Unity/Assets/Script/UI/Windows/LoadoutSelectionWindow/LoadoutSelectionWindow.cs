using Game.Agent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.UI.Windows
{
    public class LoadoutSelectionWindow : Window
    {
        [SerializeField] private CommanderDescriptionUIElement commanderDescriptionUIElement;

        public delegate void LoadoutChoosenDelegate(AgentLoadout agentLoadout);
        public event LoadoutChoosenDelegate OnLoadoutChoosen;

        private List<CommanderSelectionUIElement> commanderSelectionUIElement;
        private AgentLoadout loadout;

        private void Awake()
        {
            commanderSelectionUIElement = GetComponentsInChildren<CommanderSelectionUIElement>(true).ToList();
        }

        public void Show(AgentLoadout playerLoadout)
        {
            base.Show();

            loadout = playerLoadout;
            Refresh();
        }

        private void Refresh()
        {
            List<CommanderDefinition> commanderDefinitions = CommanderRepository.Instance.GetAll();

            for (int i = 0; i < commanderSelectionUIElement.Count; i++)
            {
                CommanderSelectionUIElement commander = commanderSelectionUIElement[i];
                CommanderDefinition commanderDefinition = commanderDefinitions.Count > i ? commanderDefinitions[i] : null;
                commander.Refresh(commanderDefinition, loadout.CommanderDefinition == commanderDefinition);

                commander.OnSelectCommander -= OnSelectCommander;
                if (commanderDefinition != null)
                    commander.OnSelectCommander += OnSelectCommander;
            }

            commanderDescriptionUIElement.Refresh(loadout.CommanderDefinition);
        }

        private void OnSelectCommander(CommanderSelectionUIElement commanderSelectionUIElement)
        {
            loadout.CommanderDefinition = commanderSelectionUIElement.CommanderDefinition;
            Refresh();
        }

        public void LoadoutChoosen()
        {
            OnLoadoutChoosen?.Invoke(loadout);
            Hide();
        }
    }
}

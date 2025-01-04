using Game.Agent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.UI.Windows
{
    public class HudWindow : Window
    {
        [SerializeField] private CommanderAbilityPanelUIElement commanderAbilityPanelUIElement;

        private List<CharacterIconUIElement> characterIconUIElements;

        private void Awake()
        {
            characterIconUIElements = GetComponentsInChildren<CharacterIconUIElement>(true).ToList();
        }

        public void OpenTechnology()
        {
            AgentEntity agentEntity = Entity.All.OfType<AgentEntity>().FirstOrDefault(x => x.Faction == FactionType.Player);
            TechnologyWindow technologyWindow = WindowManager.Instance.GetWindow<TechnologyWindow>();
            technologyWindow.Show(agentEntity);
        }

        public override void Show()
        {
            base.Show();
            commanderAbilityPanelUIElement.Refresh();

            foreach (CharacterIconUIElement characterIconUIElement in characterIconUIElements)
                characterIconUIElement.Refresh();
        }
    }
}

using Game.Agent;
using Game.Components;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.UI.Windows
{
    public class CommanderAbilityPanelUIElement : MonoBehaviour
    {
        [SerializeField] private FactionType factionType;

        private List<CommanderAbilityUIElement> commanderAbilityUIElements;

        private void Awake()
        {
            commanderAbilityUIElements = GetComponentsInChildren<CommanderAbilityUIElement>(true).ToList();

            AgentEntity agentEntity = Entity.All.OfType<AgentEntity>().FirstOrDefault(x => x.Faction == factionType);
            Caster caster = agentEntity.GetCachedComponent<Caster>();

            foreach (CommanderAbilityUIElement commanderAbilityUIElement in commanderAbilityUIElements)
                commanderAbilityUIElement.Refresh(caster);
        }
    }
}

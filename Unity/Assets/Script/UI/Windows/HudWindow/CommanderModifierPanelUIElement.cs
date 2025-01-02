using Game.Agent;
using Game.Modifier;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.UI.Windows
{
    public class CommanderModifierPanelUIElement : MonoBehaviour
    {
        [SerializeField] private FactionType faction;

        private List<CommanderModifierUIElement> commanderModifierUIElements;

        private void Awake()
        {
            commanderModifierUIElements = GetComponentsInChildren<CommanderModifierUIElement>(true).ToList();
        }

        private void Update()
        {
            AgentEntity agentEntity = Entity.All.OfType<AgentEntity>().FirstOrDefault(x => x.Faction == faction);
            List<ModifierEntity> modifiers = agentEntity.GetCachedComponent<ModifierHandler>().GetModifiers().Where((ModifierEntity x) => x.IsVisible).ToList();

            int i = 0;
            for (; i < modifiers.Count && i < commanderModifierUIElements.Count; ++i)
            {
                commanderModifierUIElements[i].gameObject.SetActive(true);
                commanderModifierUIElements[i].Refresh(modifiers[i]);
            }

            for (; i < commanderModifierUIElements.Count; ++i)
                commanderModifierUIElements[i].gameObject.SetActive(false);
        }
    }
}

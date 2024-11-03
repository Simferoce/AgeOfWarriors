using Game.Agent;
using Game.Character;
using Game.Technology;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.Windows
{
    public class CharacterIconUIElement : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private int index = 0;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI costText;

        private void Start()
        {
            AgentEntity agentEntity = Entity.All.OfType<AgentEntity>().FirstOrDefault(x => x.Faction == FactionType.Player);
            agentEntity.Technology.OnPerkAcquired += Technology_OnPerkAcquired;
            Refresh();
        }

        private void OnDestroy()
        {
            AgentEntity agentEntity = Entity.All.OfType<AgentEntity>().FirstOrDefault(x => x.Faction == FactionType.Player);
            if (agentEntity != null)
                agentEntity.Technology.OnPerkAcquired -= Technology_OnPerkAcquired;
        }

        private void Technology_OnPerkAcquired(TechnologyPerkDefinition technologyPerkDefinition)
        {
            Refresh();
        }

        private void Refresh()
        {
            AgentEntity agentEntity = Entity.All.OfType<AgentEntity>().FirstOrDefault(x => x.Faction == FactionType.Player);
            CharacterDefinition characterDefinition = agentEntity.Loadout.GetCharacterDefinitionAtIndex(index);
            if (characterDefinition == null)
            {
                gameObject.SetActive(false);
                return;
            }

            costText.text = characterDefinition.Cost.ToString();
            icon.sprite = characterDefinition.Icon;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            AgentEntity agentEntity = Entity.All.OfType<AgentEntity>().FirstOrDefault(x => x.Faction == FactionType.Player);
            agentEntity.TryQueueSpawnAgentObject(index);
        }
    }
}

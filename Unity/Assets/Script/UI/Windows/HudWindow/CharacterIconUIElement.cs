using Game.Agent;
using Game.Technology;
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
            AgentEntity agentEntity = AgentRepository.Instance.GetByFaction(FactionType.Player);
            agentEntity.Technology.OnPerkAcquired += Technology_OnPerkAcquired;
            Refresh();
        }

        private void OnDestroy()
        {
            AgentEntity agentEntity = AgentRepository.Instance.GetByFaction(FactionType.Player);
            if (agentEntity != null)
                agentEntity.Technology.OnPerkAcquired -= Technology_OnPerkAcquired;
        }

        private void Technology_OnPerkAcquired(TechnologyPerkDefinition technologyPerkDefinition)
        {
            Refresh();
        }

        private void Refresh()
        {
            AgentEntity agentEntity = AgentRepository.Instance.GetByFaction(FactionType.Player);
            AgentObjectDefinition agentObjectDefinition = agentEntity.Loadout.GetAgentObjectDefinitionAtIndex(index);
            if (agentObjectDefinition == null)
            {
                gameObject.SetActive(false);
                return;
            }

            costText.text = agentObjectDefinition.Cost.ToString();
            icon.sprite = agentObjectDefinition.Icon;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            AgentEntity agentEntity = AgentRepository.Instance.GetByFaction(FactionType.Player);
            agentEntity.TryQueueSpawnAgentObject(index);
        }
    }
}

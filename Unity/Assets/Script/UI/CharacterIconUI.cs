using Game;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class CharacterIconUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private int index = 0;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI costText;

        private void Start()
        {
            Agent.Player.Technology.OnPerkAcquired += Technology_OnPerkAcquired;
            Refresh();
        }

        private void OnDestroy()
        {
            if (Agent.Player != null)
                Agent.Player.Technology.OnPerkAcquired -= Technology_OnPerkAcquired;
        }

        private void Technology_OnPerkAcquired(TechnologyPerkDefinition technologyPerkDefinition)
        {
            Refresh();
        }

        private void Refresh()
        {
            AgentObjectDefinition agentObjectDefinition = Agent.Player.Loadout.GetAgentObjectDefinitionAtIndex(index);
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
            Agent.Player.TryQueueSpawnAgentObject(index);
        }
    }
}

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
            AgentObjectDefinition agentObjectDefinition = Agent.Player.Factory.GetAgentObjectDefinitionAtIndex(index);
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
            Agent.Player.SpawnLaneObject(index);
        }
    }
}

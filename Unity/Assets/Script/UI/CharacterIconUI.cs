using Game;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class CharacterIconUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private int index = 0;
        [SerializeField] private Image icon;

        private void Start()
        {
            AgentObjectDefinition agentObjectDefinition = Agent.Player.Factory.GetAgentObjectDefinitionAtIndex(index);
            if (agentObjectDefinition == null)
            {
                gameObject.SetActive(false);
                return;
            }

            icon.sprite = agentObjectDefinition.Icon;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Agent.Player.SpawnLaneObject(index);
        }
    }
}

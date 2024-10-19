using Game.Agent;
using TMPro;
using UnityEngine;

namespace Game.UI.Windows
{
    public class TechnologyLevelUIElement : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI textMeshProUGUI;

        public void Refresh(Agent.AgentEntity agent)
        {
            AgentEntity agentEntity = AgentRepository.Instance.GetByFaction(FactionType.Player);
            textMeshProUGUI.text = agentEntity.Technology.CurrentLevel.ToString();
        }
    }
}
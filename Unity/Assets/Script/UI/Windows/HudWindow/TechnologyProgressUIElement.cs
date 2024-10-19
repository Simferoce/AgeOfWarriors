using Game.Agent;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Windows
{
    public class TechnologyProgressUIElement : MonoBehaviour
    {
        [SerializeField] private Image foreground;

        private void Update()
        {
            AgentEntity agentEntity = AgentRepository.Instance.GetByFaction(FactionType.Player);
            foreground.fillAmount = agentEntity.Technology.CurrentTechnologyNormalized;
        }
    }
}


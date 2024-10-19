using Game.Agent;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Windows
{
    public class HealthBarBaseUIElement : MonoBehaviour
    {
        [SerializeField] private Image foreground;
        [SerializeField] private FactionType faction;

        private void Update()
        {
            AgentEntity agentEntity = AgentRepository.Instance.GetByFaction(faction);
            foreground.fillAmount = agentEntity.Base.Health / agentEntity.Base.MaxHealth;
        }
    }
}
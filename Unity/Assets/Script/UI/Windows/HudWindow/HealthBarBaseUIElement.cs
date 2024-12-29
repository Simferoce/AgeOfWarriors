using Game.Agent;
using System.Linq;
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
            AgentEntity agentEntity = Entity.All.OfType<AgentEntity>().FirstOrDefault(x => x.Faction == faction);
            foreground.fillAmount = agentEntity.Base.Health / agentEntity.Base.MaxHealth;
        }
    }
}
using Game.Agent;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Windows
{
    public class TechnologyProgressUIElement : MonoBehaviour
    {
        [SerializeField] private Image foreground;

        private void Update()
        {
            AgentEntity agentEntity = Entity.All.OfType<AgentEntity>().FirstOrDefault(x => x.Faction == FactionType.Player);
            foreground.fillAmount = agentEntity.Technology.CurrentTechnologyNormalized;
        }
    }
}


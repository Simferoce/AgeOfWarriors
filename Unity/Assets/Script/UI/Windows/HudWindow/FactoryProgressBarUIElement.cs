using Game.Agent;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Windows
{
    public class FactoryProgressBarUIElement : MonoBehaviour
    {
        [SerializeField]
        private Image bar;

        public void Update()
        {
            AgentEntity agentEntity = Entity.All.OfType<AgentEntity>().FirstOrDefault(x => x.Faction == FactionType.Player);
            bar.fillAmount = agentEntity.Factory.TimeBeforeNextProductionNormalized;
        }
    }
}
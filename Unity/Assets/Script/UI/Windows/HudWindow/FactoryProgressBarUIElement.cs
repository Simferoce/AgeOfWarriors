using Game.Agent;
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
            AgentEntity agentEntity = AgentRepository.Instance.GetByFaction(FactionType.Player);
            bar.fillAmount = agentEntity.Factory.TimeBeforeNextProductionNormalized;
        }
    }
}
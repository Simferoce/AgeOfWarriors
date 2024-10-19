using Game.UI.Windows;
using System;

namespace Game.Agent
{
    [Serializable]
    public class AgentBehaviourPlayer : AgentBehaviour
    {
        public override void OnLevelUp()
        {
            TechnologyWindow.Open(AgentRepository.Instance.GetByFaction(FactionType.Player));
        }
    }
}

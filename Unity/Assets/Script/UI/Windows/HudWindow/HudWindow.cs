using Game.Agent;

namespace Game.UI.Windows
{
    public class HudWindow : Window
    {
        public void OpenTechnology()
        {
            AgentEntity agentEntity = AgentRepository.Instance.GetByFaction(FactionType.Player);
            TechnologyWindow.Open(agentEntity);
        }
    }
}

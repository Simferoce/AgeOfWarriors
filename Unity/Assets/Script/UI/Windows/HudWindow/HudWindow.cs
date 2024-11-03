using Game.Agent;
using System.Linq;

namespace Game.UI.Windows
{
    public class HudWindow : Window
    {
        public void OpenTechnology()
        {
            AgentEntity agentEntity = Entity.All.OfType<AgentEntity>().FirstOrDefault(x => x.Faction == FactionType.Player);
            TechnologyWindow.Open(agentEntity);
        }
    }
}

using Game.Agent;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public class AgentRepository : Manager<AgentRepository>
    {
        private List<AgentEntity> agents = new List<AgentEntity>();

        public override IEnumerator InitializeAsync()
        {
            yield break;
        }

        public void Add(AgentEntity agentEntity)
        {
            agents.Add(agentEntity);
        }

        public void Remove(AgentEntity agentEntity)
        {
            agents.Remove(agentEntity);
        }

        public AgentEntity GetByFaction(FactionType faction)
        {
            return agents.FirstOrDefault(x => x.Faction == faction);
        }
    }
}

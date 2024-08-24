using Extension;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public static class TargetUtility
    {
        public static List<ITargeteable> GetTargets(AgentObject agentObject, TargetCriteria criteria, IStatisticProvider statisticProvider)
        {
            List<ITargeteable> potentialTargets = new List<ITargeteable>();
            foreach (ITargeteable targetteable in AgentObject.All.Select(x => x.GetCachedComponent<ITargeteable>()).Where(x => x != null))
            {
                if (!targetteable.IsActive)
                    continue;

                if (targetteable == agentObject.GetCachedComponent<ITargeteable>())
                    continue;

                if (!criteria.Execute(agentObject.GetCachedComponent<ITargeteable>(), targetteable, statisticProvider, agentObject.Faction, (agentObject is Character character) && character.IsConfused ? targetteable.Faction.GetConfusedFaction() : targetteable.Faction))
                    continue;

                potentialTargets.Add(targetteable);
            }

            return potentialTargets
                .OrderBy(x => x.Priority)
                .ToList();
        }
    }
}

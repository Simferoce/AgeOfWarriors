using Extension;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public static class TargetUtility
    {
        public static List<Target> GetTargets(Entity entity, TargetCriteria criteria, IStatisticProvider statisticProvider)
        {
            List<Target> potentialTargets = new List<Target>();
            foreach (Target targetteable in AgentObject.All.Select(x => x.GetCachedComponent<Target>()).Where(x => x != null))
            {
                if (!(targetteable.Entity as AgentObject).IsActive)
                    continue;

                if (targetteable == entity.GetCachedComponent<Target>())
                    continue;

                if (!criteria.Execute(entity.GetCachedComponent<Target>(), targetteable, statisticProvider, (entity as AgentObject).Faction, (entity is Character character) && character.IsConfused ? (targetteable.Entity as AgentObject).Faction.GetConfusedFaction() : (targetteable.Entity as AgentObject).Faction))
                    continue;

                potentialTargets.Add(targetteable);
            }

            return potentialTargets
                .OrderBy(x => (x.Entity as AgentObject).Priority)
                .ToList();
        }
    }
}

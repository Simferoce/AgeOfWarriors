using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public static class TargetUtility
    {
        public static List<Target> GetTargets(Entity entity, TargetCriteria criteria, IStatisticContext statisticProvider)
        {
            List<Target> potentialTargets = new List<Target>();
            foreach (Target targetteable in AgentObject.All.Select(x => x.GetCachedComponent<Target>()).Where(x => x != null))
            {
                if (!(targetteable.Entity as AgentObject).IsActive)
                    continue;

                if (targetteable.Entity == entity)
                    continue;

                if (!criteria.Execute(entity, targetteable.Entity))
                    continue;

                potentialTargets.Add(targetteable);
            }

            return potentialTargets
                .OrderBy(x => (x.Entity as AgentObject).Priority)
                .ToList();
        }
    }
}

using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public static class TargetUtility
    {
        public static List<Target> GetTargets(System.Func<Target, bool> predicat)
        {
            List<Target> potentialTargets = new List<Target>();
            foreach (Target targetteable in AgentObject.All.Select(x => x.GetCachedComponent<Target>()).Where(x => x != null))
            {
                if (!targetteable.Entity.IsActive)
                    continue;

                if (!predicat.Invoke(targetteable))
                    continue;

                potentialTargets.Add(targetteable);
            }

            return potentialTargets
                .OrderBy(x => (x.Entity as AgentObject).Priority)
                .ToList();
        }
    }
}

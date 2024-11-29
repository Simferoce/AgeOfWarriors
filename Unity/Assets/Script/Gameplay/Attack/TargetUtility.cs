using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public static class TargetUtility
    {
        public static List<ITargeteable> GetTargets(System.Func<ITargeteable, bool> predicat)
        {
            List<ITargeteable> potentialTargets = new List<ITargeteable>();
            foreach (ITargeteable targetteable in AgentObject.All.Select(x => x as ITargeteable).Where(x => x != null))
            {
                if (!targetteable.IsActive)
                    continue;

                if (!predicat.Invoke(targetteable))
                    continue;

                potentialTargets.Add(targetteable);
            }

            return potentialTargets
                .OrderBy(x => x.Priority)
                .ToList();
        }
    }
}

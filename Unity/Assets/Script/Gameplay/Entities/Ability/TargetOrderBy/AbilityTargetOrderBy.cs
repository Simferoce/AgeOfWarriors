using Game.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Ability
{
    [Serializable]
    public abstract class AbilityTargetOrderBy
    {
        public abstract IOrderedEnumerable<Target> OrderBy(IEnumerable<Target> targets);
    }
}

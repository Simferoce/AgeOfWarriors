using Game.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Ability
{
    [Serializable]
    public abstract class AbilityTargetOrderBy
    {
        protected AbilityEntity ability;

        public virtual void Initialize(AbilityEntity ability)
        {
            this.ability = ability;
        }

        public abstract IOrderedEnumerable<Target> OrderBy(IEnumerable<Target> targets);
    }
}

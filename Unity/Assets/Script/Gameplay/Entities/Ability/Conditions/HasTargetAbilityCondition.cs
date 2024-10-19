using Game.Agent;
using Game.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Ability
{
    [Serializable]
    public class HasTargetAbilityCondition : AbilityCondition
    {
        [SerializeReference, SubclassSelector] private AbilityTargetFilter filter;
        [SerializeField] private int count = 1;

        public List<Target> Targets { get; set; } = new List<Target>();

        public override bool Validate()
        {
            bool changed = base.Validate();
            if (filter != null)
                changed |= filter.Validate();
            return changed;
        }

        public override bool Execute()
        {
            Targets.Clear();
            Targets.AddRange(GetTargets(ability, filter));
            return Targets.Count >= count;
        }

        public static List<Target> GetTargets(AbilityEntity ability, AbilityTargetFilter filter)
        {
            List<Target> potentialTargets = new List<Target>();
            foreach (Target targetteable in EntityRepository.Instance.GetByType<AgentObject>().Select(x => x.GetCachedComponent<Target>()).Where(x => x != null))
            {
                if (!(targetteable.Entity as AgentObject).IsActive)
                    continue;

                if (targetteable.Entity == ability.Caster.Entity)
                    continue;

                if (!filter.Execute(ability, targetteable.Entity))
                    continue;

                potentialTargets.Add(targetteable);
            }

            return potentialTargets
                .OrderBy(x => (x.Entity as AgentObject).Priority)
                .ToList();
        }
    }
}

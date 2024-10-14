using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class HasTargetAbilityCondition : AbilityCondition
    {
        [SerializeReference, SubclassSelector] private AbilityTargetCriteria criteria;
        [SerializeField] private int count = 1;

        public List<Target> Targets { get; set; } = new List<Target>();

        public override bool Execute()
        {
            Targets.Clear();
            Targets.AddRange(GetTargets(ability, criteria));
            return Targets.Count >= count;
        }

        public static List<Target> GetTargets(Ability ability, AbilityTargetCriteria criteria)
        {
            List<Target> potentialTargets = new List<Target>();
            foreach (Target targetteable in AgentObject.All.Select(x => x.GetCachedComponent<Target>()).Where(x => x != null))
            {
                if (!(targetteable.Entity as AgentObject).IsActive)
                    continue;

                if (targetteable.Entity == ability.Caster.Entity)
                    continue;

                if (!criteria.Execute(ability, targetteable.Entity))
                    continue;

                potentialTargets.Add(targetteable);
            }

            return potentialTargets
                .OrderBy(x => (x.Entity as AgentObject).Priority)
                .ToList();
        }
    }
}

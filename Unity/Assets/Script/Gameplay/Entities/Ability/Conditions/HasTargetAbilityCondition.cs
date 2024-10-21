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
        [SerializeField] private Vector2Int countInterval = Vector2Int.one;
        [SerializeReference, SubclassSelector] private AbilityTargetFilter filter;
        [SerializeReference, SubclassSelector] private List<AbilityTargetOrderBy> orderBy;

        public List<Target> Targets { get; set; } = new List<Target>();

        public override void Initialize(AbilityEntity ability)
        {
            base.Initialize(ability);
            if (filter != null)
                filter.Initialize(ability);
        }

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
            Targets.AddRange(GetTargets().Take(countInterval.y));
            return Targets.Count >= countInterval.x;
        }

        public List<Target> GetTargets()
        {
            List<Target> potentialTargets = new List<Target>();
            foreach (Target targetteable in EntityRepository.Instance.GetByType<AgentObject>().Select(x => x.GetCachedComponent<Target>()).Where(x => x != null))
            {
                if (!(targetteable.Entity as AgentObject).IsActive)
                    continue;

                if (!filter.Execute(ability, targetteable.Entity))
                    continue;

                potentialTargets.Add(targetteable);
            }

            IEnumerable<Target> orderByTargets = potentialTargets;
            foreach (AbilityTargetOrderBy orderBy in orderBy)
                orderByTargets = orderBy.OrderBy(orderByTargets);

            return orderByTargets.ToList();
        }
    }
}

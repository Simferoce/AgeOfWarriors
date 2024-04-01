using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class GraspAbilityEffect : AbilityEffect, ILingeringAbilityEffect
    {
        [SerializeField, Range(0, 3)] private float distanceBaseOnReach;
        [SerializeField] private float destinationDistance;
        [SerializeField] private float duration = 0.3f;
        [SerializeField] private float staggerDuration = 0.5f;
        [SerializeField, Range(0, 1)] private float damping = 0.05f;

        private float startedAt;

        public override bool CanBeApplied()
        {
            return GetPotentialTargets().Count >= 2;
        }

        public override void Apply()
        {
            List<IDisplaceable> potentialTargets = GetPotentialTargets();
            foreach (IDisplaceable target in potentialTargets)
            {
                if (target is IAttackable attackable)
                    attackable.Stagger(staggerDuration);
            }

            startedAt = Time.time;
        }

        public bool Update(Character character)
        {
            List<IDisplaceable> potentialTargets = GetPotentialTargets();
            Vector3 destination = character.transform.position + character.transform.right * destinationDistance;

            foreach (IDisplaceable target in potentialTargets)
                target.Displace(Vector3.Lerp(target.Position, destination, damping) - target.Position);

            return Time.time - startedAt > duration;
        }

        public List<IDisplaceable> GetPotentialTargets()
        {
            return AgentObject.All.OfType<IDisplaceable>()
                .Where(x => x != (IDisplaceable)character
                    && x.IsActive
                    && x.IsDisplaceable
                    && x.Faction != character.Faction
                    && Vector3.Distance(x.Position, character.Position) < character.Reach * distanceBaseOnReach)
                .ToList();
        }
    }
}

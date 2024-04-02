using System;
using System.Linq;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class GraspAbilityEffect : AbilityEffect, ILingeringAbilityEffect
    {
        [SerializeField] private float destinationDistance;
        [SerializeField] private float duration = 0.3f;
        [SerializeField] private float staggerDuration = 0.5f;
        [SerializeField, Range(0, 1)] private float damping = 0.05f;

        private float startedAt;

        public override void Apply()
        {
            foreach (IDisplaceable target in Ability.Targets.Cast<IDisplaceable>().ToList())
            {
                if (target is IAttackable attackable)
                    attackable.Stagger(staggerDuration);
            }

            startedAt = Time.time;
        }

        public bool Update(Character character)
        {
            Vector3 destination = character.transform.position + character.transform.right * destinationDistance;

            foreach (IDisplaceable target in Ability.Targets.Cast<IDisplaceable>().ToList())
                target.Displace(Vector3.Lerp(target.CenterPosition, destination, damping) - target.CenterPosition);

            return Time.time - startedAt > duration;
        }
    }
}

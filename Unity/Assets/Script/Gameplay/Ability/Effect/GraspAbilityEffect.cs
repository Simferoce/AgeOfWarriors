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
        [SerializeField] private StatisticReference<float> staggerDuration;
        [SerializeField, Range(0, 1)] private float damping = 0.05f;

        private float startedAt;

        public override void Apply()
        {
            float duration = staggerDuration.GetValueOrThrow(Ability);
            foreach (IDisplaceable target in Ability.Targets.Cast<IDisplaceable>().ToList())
            {
                if (target is IStaggerable staggerable)
                    staggerable.Stagger(duration);
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

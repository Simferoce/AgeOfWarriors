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
        [SerializeField] private StaggerModifierDefinition staggerModifierDefinition;

        private float startedAt;

        public override void Apply()
        {
            float duration = staggerDuration.GetValueOrThrow(new Context() { { "ability", Ability } });
            foreach (Character target in Ability.Targets.Cast<Character>().ToList())
            {
                if (target.TryGetCachedComponent<IModifiable>(out IModifiable targetModifiable))
                    targetModifiable.AddModifier(new StaggerModifierDefinition.Modifier(targetModifiable, staggerModifierDefinition, duration, Ability.Character));
            }

            startedAt = Time.time;
        }

        public bool Update(Character character)
        {
            Vector3 destination = character.transform.position + character.transform.right * destinationDistance;

            foreach (Character target in Ability.Targets.Cast<Character>().ToList())
                target.Displace(Vector3.Lerp(target.CenterPosition, destination, damping) - target.CenterPosition);

            return Time.time - startedAt > duration;
        }
    }
}

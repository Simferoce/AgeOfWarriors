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
        [SerializeField] private StatisticReference staggerDuration;
        [SerializeField, Range(0, 1)] private float damping = 0.05f;
        [SerializeField] private StaggerModifierDefinition staggerModifierDefinition;

        private float startedAt;

        public override void Apply()
        {
            float duration = staggerDuration;
            foreach (Character target in Ability.Targets.Cast<Character>().ToList())
            {
                if (target.TryGetCachedComponent<ModifierHandler>(out ModifierHandler targetModifiable))
                    targetModifiable.AddModifier(new StaggerModifierDefinition.Modifier(targetModifiable, staggerModifierDefinition, duration, Ability.Caster as IModifierSource));
            }

            startedAt = Time.time;
        }

        public bool Update(Caster caster)
        {
            Vector3 destination = (caster.Entity as AgentObject).transform.position + (caster.Entity as AgentObject).Direction * destinationDistance * Vector3.right;

            foreach (Character character in Ability.Targets.Cast<Character>().ToList())
                character.Displace(Vector3.Lerp(character.GetCachedComponent<Target>().CenterPosition, destination, damping) - character.GetCachedComponent<Target>().CenterPosition);

            return Time.time - startedAt > duration;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class InstantanousCharacterAbility : CharacterAbility
    {
        [SerializeReference, SubclassSelector] private List<AbilityCondition> conditions = new List<AbilityCondition>();
        [SerializeReference, SubclassSelector] private List<AbilityEffect> effects = new List<AbilityEffect>();

        public override List<IAttackable> Targets => (conditions.FirstOrDefault(x => x is HasTargetAbilityCondition) as HasTargetAbilityCondition)?.Targets ?? base.Targets;

        public InstantanousCharacterAbility() : base()
        {

        }


        public InstantanousCharacterAbility(InstantanousCharacterAbility other) : base(other)
        {
            conditions = other.conditions.Select(x => x.Clone()).ToList();
            effects = other.effects.Select(x => x.Clone()).ToList();
        }

        public override bool CanUse()
        {
            return conditions.All(x => x.Execute()) && effects.All(x => x.CanBeApplied());
        }

        public override void Dispose()
        {

        }

        public override void Initialize(Character character)
        {
            base.Initialize(character);

            foreach (AbilityCondition condition in conditions)
                condition.Initialize(this);

            foreach (AbilityEffect effect in effects)
                effect.Initialize(this);
        }

        public override void Update()
        {
        }

        public override void Use()
        {
            foreach (AbilityCondition condition in conditions)
                condition.OnAbilityStarted();

            foreach (AbilityEffect effect in effects)
                effect.OnAbilityStarted();

            foreach (AbilityEffect effect in effects)
                effect.Apply();

            End();
        }

        private void End()
        {
            foreach (AbilityEffect effect in effects)
                effect.OnAbilityEnded();

            foreach (AbilityCondition condition in conditions)
                condition.OnAbilityEnded();
        }

        public override void Interrupt()
        {
            foreach (AbilityEffect effect in effects)
                effect.Interrupt();

            foreach (AbilityCondition condition in conditions)
                condition.Interrupt();
        }

        public override CharacterAbility Clone()
        {
            return new InstantanousCharacterAbility(this);
        }
    }
}

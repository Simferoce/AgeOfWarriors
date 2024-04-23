using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public abstract class InstantanousCharacterAbility<T> : Ability<T>
        where T : AbilityDefinition
    {
        [SerializeReference, SubclassSelector] private List<AbilityCondition> conditions = new List<AbilityCondition>();
        [SerializeReference, SubclassSelector] private List<AbilityEffect> effects = new List<AbilityEffect>();

        public override List<ITargeteable> Targets => (conditions.FirstOrDefault(x => x is HasTargetAbilityCondition) as HasTargetAbilityCondition)?.Targets ?? base.Targets;

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

        public override void Tick()
        {
        }

        public override void Apply()
        {
            foreach (AbilityEffect effect in effects)
                effect.Apply();

            PublishEffectApplied();
        }

        public override void Use()
        {
            IsCasting = true;
            foreach (AbilityCondition condition in conditions)
                condition.OnAbilityStarted();

            foreach (AbilityEffect effect in effects)
                effect.OnAbilityStarted();

            Apply();

            End();
        }

        private void End()
        {
            foreach (AbilityEffect effect in effects)
                effect.OnAbilityEnded();

            foreach (AbilityCondition condition in conditions)
                condition.OnAbilityEnded();

            IsCasting = false;
        }

        public override void Interrupt()
        {
            foreach (AbilityEffect effect in effects)
                effect.Interrupt();

            foreach (AbilityCondition condition in conditions)
                condition.Interrupt();

            IsCasting = false;
        }
    }
}

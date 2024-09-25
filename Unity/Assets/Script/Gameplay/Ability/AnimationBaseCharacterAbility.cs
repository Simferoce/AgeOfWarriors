using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace Game
{
    public class AnimationBaseCharacterAbility : Ability
    {
        [SerializeReference, SubclassSelector] private List<AbilityCondition> conditions = new List<AbilityCondition>();
        [SerializeReference, SubclassSelector] private List<AbilityEffect> effects = new List<AbilityEffect>();

        [Header("Animation Parameters")]
        [SerializeField, FormerlySerializedAs("parameter")] private string trigger;

        public override List<Target> Targets => (conditions.FirstOrDefault(x => x is HasTargetAbilityCondition) as HasTargetAbilityCondition)?.Targets ?? base.Targets;
        public override bool IsActive { get => IsCasting || IsLingering; }
        public bool IsLingering { get; set; } = false;

        private Animated animated;

        public override void Initialize(Caster caster)
        {
            base.Initialize(caster);

            animated = caster.Entity.GetCachedComponent<Animated>();
            Assert.IsNotNull(animated, "Cannot cast an animated ability if the caster does not own an animated component.");

            foreach (AbilityCondition condition in conditions)
                condition.Initialize(this);

            foreach (AbilityEffect effect in effects)
                effect.Initialize(this);
        }

        public override bool CanUse()
        {
            return conditions.All(x => x.Execute()) && effects.All(x => x.CanBeApplied()) && IsCasting == false && IsLingering == false;
        }

        public override void InternalUse()
        {
            FactionWhenUsed = (Caster.Entity as AgentObject).Faction;

            Caster.BeginCast();
            animated.SetTrigger(trigger);
            Caster.LastAbilityUsed = Time.time;
            IsCasting = true;

            foreach (AbilityCondition condition in conditions)
                condition.OnAbilityStarted();

            foreach (AbilityEffect effect in effects)
                effect.OnAbilityStarted();

            animated.OnAbilityUsed += OnAnimatorEventAbilityUsed;
            AnimatorEventChannel.Subscribe(animated.Animator, AnimatorEventChannel.Event.OnExit, AnimatorEventChannel.Id.Ability, OnCastEnded);
        }

        public override void Tick()
        {
            if (IsCasting == true)
                return;

            bool end = true;
            foreach (ILingeringAbilityEffect lingeringAbilityEffect in effects.OfType<ILingeringAbilityEffect>())
            {
                end &= lingeringAbilityEffect.Update(Caster);
            }

            if (end)
                End();
        }

        public override void Apply()
        {
            foreach (AbilityEffect effect in effects)
                effect.Apply();

            PublishEffectApplied();
        }

        protected void OnAnimatorEventAbilityUsed()
        {
            Apply();
        }

        private void OnCastEnded()
        {
            IsCasting = false;

            if (effects.OfType<ILingeringAbilityEffect>().Count() == 0)
                End();
            else
                IsLingering = true;

            Caster.EndCast();
        }

        public override void Dispose()
        {
            animated.OnAbilityUsed -= OnAnimatorEventAbilityUsed;
            AnimatorEventChannel.Unsubscribe(animated.Animator, AnimatorEventChannel.Event.OnExit, AnimatorEventChannel.Id.Ability, OnCastEnded);
        }

        private void End()
        {
            IsLingering = false;

            foreach (AbilityCondition condition in conditions)
                condition.OnAbilityEnded();

            foreach (AbilityEffect effect in effects)
                effect.OnAbilityEnded();

            Dispose();
        }

        public override void Interrupt()
        {
            Caster.EndCast();
            IsLingering = false;
            IsCasting = false;

            foreach (AbilityCondition condition in conditions)
                condition.Interrupt();

            foreach (AbilityEffect effect in effects)
                effect.Interrupt();

            Dispose();
        }
    }
}

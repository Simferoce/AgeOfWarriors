using UnityEngine;
using UnityEngine.Assertions;

namespace Game
{
    public abstract class AnimationBaseCharacterAbility<T> : Ability<T>
        where T : AbilityDefinition
    {
        public override bool IsActive { get => IsCasting; }

        private Animated animated;
        private string trigger;

        public AnimationBaseCharacterAbility(T definition, string trigger = "")
            : base(definition)
        {
            this.trigger = trigger;
        }

        public override void Initialize(Caster caster)
        {
            base.Initialize(caster);

            animated = caster.GetCachedComponent<IAnimated>()?.Animated;
            Assert.IsNotNull(animated, "Cannot cast an animated ability if the caster does not own an animated component.");
        }

        public override bool CanUse()
        {
            return Time.time - Caster.LastAbilityUsed > Caster.AgentObject.GetStatisticOrDefault<float>("attack_speed");
        }

        public override void InternalApply() { }

        public override void InternalUse()
        {
            Caster.BeginCast();
            animated.SetTrigger(trigger);
            IsCasting = true;

            animated.OnAbilityUsed += OnAnimatorEventAbilityUsed;
            AnimatorEventChannel.Subscribe(animated.Animator, AnimatorEventChannel.Event.OnExit, AnimatorEventChannel.Id.Ability, OnCastEnded);
        }

        public override void Tick()
        {

        }

        protected void OnAnimatorEventAbilityUsed()
        {
            Apply();
        }

        private void OnCastEnded()
        {
            IsCasting = false;
            Caster.EndCast();
            Dispose();
        }

        public override void Dispose()
        {
            animated.OnAbilityUsed -= OnAnimatorEventAbilityUsed;
            AnimatorEventChannel.Unsubscribe(animated.Animator, AnimatorEventChannel.Event.OnExit, AnimatorEventChannel.Id.Ability, OnCastEnded);
        }

        public override void Interrupt()
        {
            Caster.EndCast();
            IsCasting = false;

            Dispose();
        }
    }
}

using Game.Agent;
using Game.Animation;
using Game.Components;
using Game.Statistics;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
#endif
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace Game.Ability
{
    public class AbilityAnimationBaseCharacter : AbilityEntity
    {
        [SerializeReference, SubclassSelector] private List<AbilityEffect> effects = new List<AbilityEffect>();

        [Header("Animation Parameters")]
        [SerializeField, FormerlySerializedAs("parameter")] private string trigger;

        public override List<Target> Targets => (conditions.FirstOrDefault(x => x is HasTargetAbilityCondition) as HasTargetAbilityCondition)?.Targets ?? base.Targets;
        public bool IsLingering { get; set; } = false;

        private Animated animated;
        private bool applied = false;

        public override void Initialize(Caster caster)
        {
            base.Initialize(caster);

            animated = caster.Entity.GetCachedComponent<Animated>();
            Assert.IsNotNull(animated, "Cannot cast an animated ability if the caster does not own an animated component.");

            foreach (AbilityEffect effect in effects)
                effect.Initialize(this);
        }

        public override bool CanUse()
        {
            return base.CanUse() && effects.All(x => x.CanBeApplied()) && IsCasting == false;
        }

        public override void InternalUse()
        {
            Faction = Caster.Entity.GetCachedComponent<AgentIdentity>().Faction;

            Caster.BeginCast();
            animated.Play(trigger);
            Caster.LastAbilityUsed = Time.time;
            IsCasting = true;
            IsLingering = false;
            applied = false;

            foreach (AbilityCondition condition in conditions)
                condition.OnAbilityStarted();

            foreach (AbilityEffect effect in effects)
                effect.OnAbilityStarted();

            animated.OnAbilityUsed += OnAnimatorEventAbilityUsed;
            AnimatorEventChannel.Subscribe(animated.Animator, AnimatorEventChannel.Event.OnExit, AnimatorEventChannel.Id.Ability, OnCastEnded);
        }

        public override void Tick()
        {
            base.Tick();
            animated.SetSpeed(Caster.Entity[StatisticDefinitionRegistry.Instance.AttackSpeed] * animated.GetCurrentAnimationClipLength());

            if (applied == false)
                return;

            bool end = true;
            foreach (IContinousAbilityEffect continousAbilityEffect in effects.OfType<IContinousAbilityEffect>())
                end &= continousAbilityEffect.Update(Caster);

            if (IsLingering && end)
                End();
        }

        public override void Apply()
        {
            applied = true;
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
            if (effects.OfType<IContinousAbilityEffect>().Count() == 0)
                End();
            else
                IsLingering = true;
        }

        public override void Dispose()
        {
            animated.OnAbilityUsed -= OnAnimatorEventAbilityUsed;
            AnimatorEventChannel.Unsubscribe(animated.Animator, AnimatorEventChannel.Event.OnExit, AnimatorEventChannel.Id.Ability, OnCastEnded);
        }

        private void End()
        {
            animated.SetSpeed(1f);
            Caster.EndCast();
            IsCasting = false;

            foreach (AbilityCondition condition in conditions)
                condition.OnAbilityEnded();

            foreach (AbilityEffect effect in effects)
                effect.OnAbilityEnded();

            Dispose();
        }

        public override void Interrupt()
        {
            Caster.EndCast();
            IsCasting = false;

            foreach (AbilityCondition condition in conditions)
                condition.Interrupt();

            foreach (AbilityEffect effect in effects)
                effect.Interrupt();

            Dispose();
        }
    }
}

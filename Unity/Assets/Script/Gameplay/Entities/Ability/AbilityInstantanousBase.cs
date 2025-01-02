using Game.Components;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Ability
{
    public class AbilityInstantanousBase : AbilityEntity
    {
        [SerializeReference, SubclassSelector] private List<AbilityEffect> effects = new List<AbilityEffect>();

        public override void Initialize(Caster caster)
        {
            base.Initialize(caster);

            foreach (AbilityEffect effect in effects)
                effect.Initialize(this);
        }

        public override bool CanUse()
        {
            return base.CanUse();
        }

        public override void Apply()
        {
            foreach (AbilityEffect effect in effects)
                effect.Apply();
        }

        public override void Dispose()
        {
        }

        public override void InternalUse()
        {
            Caster.BeginCast();
            Apply();

            Caster.EndCast();

            foreach (AbilityCondition condition in conditions)
                condition.OnAbilityEnded();

            foreach (AbilityEffect effect in effects)
                effect.OnAbilityEnded();
        }

        public override void Interrupt()
        {
        }
    }
}

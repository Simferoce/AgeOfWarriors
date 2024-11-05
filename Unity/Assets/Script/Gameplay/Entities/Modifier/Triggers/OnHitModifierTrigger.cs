using Game.Components;
using System;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class OnHitModifierTrigger : ModifierTrigger
    {
        [SerializeReference, SubclassSelector] private OnHitModifierTriggerCondition condition;

        private Attackable attackable;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            attackable = modifier.Target.Entity.GetCachedComponent<Attackable>();
            attackable.OnDamageTaken += OnDamageTaken;
        }

        private void OnDamageTaken(AttackResult result, Attackable receiver)
        {
            if (condition == null || condition.Execute(result, receiver))
                Trigger();
        }

        public override void Dispose()
        {
            base.Dispose();
            attackable.OnDamageTaken -= OnDamageTaken;
        }
    }
}

using Game.Components;
using System;
using System.Linq;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class DamageAbsorbedByDefenseModifierBehaviour : ModifierBehaviour
    {
        private Attackable attackable;
        private StackModifierBehaviour stackModifierBehaviour;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            attackable = modifier.Target.Entity.GetCachedComponent<Attackable>();
            stackModifierBehaviour = modifier.Behaviours.FirstOrDefault(x => x is StackModifierBehaviour) as StackModifierBehaviour;
            if (stackModifierBehaviour == null)
            {
                Debug.LogError($"Expecting the modifier \"{modifier}\" to have a behaviour of type \"{nameof(StackModifierBehaviour)}\"");
                return;
            }

            attackable.OnDamageTaken += Attackable_OnDamageTaken;
        }

        private void Attackable_OnDamageTaken(AttackResult result, Attackable receiver)
        {
            stackModifierBehaviour.IncreaseStack(result.DefenseDamagePrevented);
        }

        public override void Dispose()
        {
            base.Dispose();
            attackable.OnDamageTaken -= Attackable_OnDamageTaken;
        }
    }
}

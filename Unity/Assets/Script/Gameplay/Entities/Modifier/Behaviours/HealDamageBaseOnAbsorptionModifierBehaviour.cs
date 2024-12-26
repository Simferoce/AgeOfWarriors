using Game.Components;
using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class HealDamageBaseOnAbsorptionModifierBehaviour : ModifierBehaviour
    {
        [SerializeField] private StatisticReference ratioHeal;

        private Attackable attackable;
        private IHealable healable;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            attackable = modifier.Target.Entity.GetCachedComponent<Attackable>();
            healable = modifier.Target.Entity as IHealable;
            attackable.ShieldHandler.OnAbsorbed += OnAbsorbed;
            ratioHeal.Initialize(modifier);
        }

        private void OnAbsorbed(float amount)
        {
            healable.Heal(amount * ratioHeal.Get());
        }

        public override void Dispose()
        {
            base.Dispose();
            attackable.ShieldHandler.OnAbsorbed -= OnAbsorbed;
        }
    }
}

using Game.Components;
using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class ShieldModifierBehaviour : ModifierBehaviour, IShield, IModifierStack
    {
        [SerializeField] private StatisticReference amount;

        public float Remaining { get => amount.Get(); }
        public float CurrentStack { get => Remaining; set => amount.Get().Set(value); }
        public float InitialAmount => initialAmount;

        private Attackable attackable;
        private float initialAmount;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            amount.Initialize(modifier);

            attackable = modifier.Target.Entity.GetCachedComponent<Attackable>();
            attackable.ShieldHandler.Add(this);

            initialAmount = Remaining;
        }

        public float Absorb(float damage)
        {
            float before = Remaining;
            amount.Get().Set(amount.Get() - damage);
            damage -= before;

            amount.Get().Set(Mathf.Max(Remaining, 0));
            damage = Mathf.Max(damage, 0);

            if (Remaining == 0)
                modifier.Kill();

            return damage;
        }

        public override void Dispose()
        {
            base.Dispose();
            attackable.ShieldHandler.Remove(this);
        }
    }
}

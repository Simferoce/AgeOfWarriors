using Game.Components;
using Game.Statistics;
using System;
using System.Linq;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class HealModifierEffect : ModifierEffect
    {
        [SerializeReference, SubclassSelector] private ModifierTarget target;
        [SerializeField] private StatisticReference heal;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            target.Initialize(modifier);
            heal.Initialize(modifier);
        }

        public override void Execute()
        {
            foreach (Entity entity in target.GetTargets().OfType<Entity>())
            {
                if (entity is IHealable healable)
                {
                    healable.Heal(heal.Get());
                }
            }
        }
    }
}

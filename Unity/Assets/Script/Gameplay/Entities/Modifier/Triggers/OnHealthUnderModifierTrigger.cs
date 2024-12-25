using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class OnHealthUnderModifierTrigger : ModifierTrigger
    {
        [SerializeField] private StatisticReference threshold;

        private bool triggered = false;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            threshold.Initialize(modifier);
        }

        public override void Update()
        {
            base.Update();

            if (triggered)
                return;

            if (modifier.Target.Entity[StatisticDefinitionRegistry.Instance.Health] / modifier.Target.Entity[StatisticDefinitionRegistry.Instance.MaxHealth] < threshold.Get())
            {
                Trigger();
                triggered = true;
            }
        }
    }
}

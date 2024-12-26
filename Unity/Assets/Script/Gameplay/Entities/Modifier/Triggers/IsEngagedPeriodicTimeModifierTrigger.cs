using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class IsEngagedPeriodicTimeModifierTrigger : ModifierTrigger
    {
        [SerializeField] private StatisticReference period;

        private float lastPeriod;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            period.Initialize(modifier);
            lastPeriod = Time.time;
        }

        public override void Update()
        {
            base.Update();

            if (!modifier.Target.Entity.StatisticRepository.TryGet("engaged", out Statistic engagedStatistic) || !engagedStatistic)
                lastPeriod = Time.time;

            if (Time.time - lastPeriod >= period.Get())
            {
                Trigger();
                lastPeriod = Time.time;
            }
        }
    }
}

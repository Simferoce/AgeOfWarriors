using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class TimeModifierBehaviour : ModifierBehaviour, IModifierDuration
    {
        [SerializeReference, SubclassSelector]
        private Statistic duration;

        public float Duration { get => /*duration.GetValue<float>(modifier)*/0f; }
        public float RemaingDuration { get => Time.time - startedAt; }

        private float startedAt = 0f;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            startedAt = Time.time;
        }

        public override void Refresh()
        {
            startedAt = Time.time;
        }

        public override Result Update()
        {
            if (Time.time - startedAt > Duration)
                return Result.Dead;

            return Result.Alive;
        }

        public float GetPercentageRemainingDuration()
        {
            return Mathf.Clamp01(RemaingDuration / Duration);
        }
    }
}

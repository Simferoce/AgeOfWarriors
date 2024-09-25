using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class TimeModifierBehaviour : ModifierBehaviour, IModifierDuration
    {
        private float duration = 0f;
        private float startedAt = 0f;
        public float Duration { get => duration; }
        public float RemaingDuration { get => Time.time - startedAt; }

        public TimeModifierBehaviour(float duration)
        {
            this.duration = duration;
        }

        public override void Initialize()
        {
            startedAt = Time.time;
        }

        public override void Refresh()
        {
            startedAt = Time.time;
        }

        public override bool Update()
        {
            return Time.time - startedAt > duration;
        }

        public float GetPercentageRemainingDuration()
        {
            throw new NotImplementedException();
        }
    }
}

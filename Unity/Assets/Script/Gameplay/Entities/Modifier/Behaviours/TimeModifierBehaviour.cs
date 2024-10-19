using System;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class TimeModifierBehaviour : ModifierBehaviour, IModifierDuration
    {
        [SerializeReference, SubclassSelector]
        private ModifierStatistic duration;

        public float Duration { get => duration.GetValue<float>(modifier); }
        public float RemaingDuration { get => Time.time - startedAt; }

        private float startedAt = 0f;

        public override void Initialize(ModifierEntity modifier)
        {
            startedAt = Time.time;
        }

        public override void Refresh()
        {
            startedAt = Time.time;
        }

        //public override bool Update()
        //{
        //    return Time.time - startedAt > duration;
        //}

        public float GetPercentageRemainingDuration()
        {
            throw new NotImplementedException();
        }
    }
}

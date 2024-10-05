using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ModifierBehaviourTime : ModifierBehaviour, IModifierDuration
    {
        [SerializeReference, SubclassSelector]
        private StatisticReference duration;

        public float Duration { get => duration; }
        public float RemaingDuration { get => Time.time - startedAt; }

        private float startedAt = 0f;

        public override void Initialize(Modifier modifier)
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

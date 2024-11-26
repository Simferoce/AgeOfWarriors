using System;
using UnityEngine;

namespace Game.Statistics
{
    [Serializable]
    public class EntityReferenceValue<T> : Value<T>
    {
        [SerializeField] private StatisticReference<T> reference;

        public StatisticReference<T> Reference { get => reference; set => reference = value; }

        public override void Initialize(Entity owner)
        {
            base.Initialize(owner);
            reference.Initialize(owner);
        }

        public override T GetValue()
        {
            return reference.GetOrThrow().GetModifiedValue(Context.Empty);
        }

        public override bool TryGetDescription(out string description)
        {
            description = null;
            return false;
        }
    }
}

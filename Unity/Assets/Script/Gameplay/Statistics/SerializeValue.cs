using System;
using UnityEngine;

namespace Game.Statistics
{
    [Serializable]
    public class SerializeValueFloat : SerializeValue<float> { }

    [Serializable]
    public class SerializeValue<T> : Value<T>
    {
        [SerializeField] private T value;

        public override bool ExpressiveDescription => false;

        public T Value { get => value; set => this.value = value; }

        public override T GetValue()
        {
            return value;
        }

        public override bool TryGetDescription(out string description)
        {
            description = string.Empty;
            return false;
        }
    }
}

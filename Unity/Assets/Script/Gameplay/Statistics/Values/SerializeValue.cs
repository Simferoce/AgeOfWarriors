using System;
using UnityEngine;

namespace Game.Statistics
{
    [Serializable]
    public class SerializeValueFloat : SerializeValue<float>
    {
        public SerializeValueFloat(float value) : base(value)
        {
        }
    }

    [Serializable]
    public class SerializeValueBool : SerializeValue<bool>
    {
        public SerializeValueBool(bool value) : base(value)
        {
        }
    }

    [Serializable]
    public class SerializeValueInteger : SerializeValue<int>
    {
        public SerializeValueInteger(int value) : base(value)
        {
        }
    }

    [Serializable]
    public abstract class SerializeValue<T> : Value<T>
    {
        [SerializeField] private T value;

        public T Value { get => value; set => this.value = value; }

        public SerializeValue(T value)
        {
            this.value = value;
        }

        public override T GetValue()
        {
            return value;
        }
    }
}

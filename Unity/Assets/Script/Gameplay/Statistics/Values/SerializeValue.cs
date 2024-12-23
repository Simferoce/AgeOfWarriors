using System;
using UnityEngine;

namespace Game.Statistics
{
    [Serializable]
    public class SerializeValueFloat : SerializeValue<float>
    {
        public SerializeValueFloat()
        {
        }

        public SerializeValueFloat(float value) : base(value)
        {
        }
    }

    [Serializable]
    public class SerializeValueBool : SerializeValue<bool>
    {
        public SerializeValueBool()
        {
        }

        public SerializeValueBool(bool value) : base(value)
        {
        }
    }

    [Serializable]
    public class SerializeValueInteger : SerializeValue<int>
    {
        public SerializeValueInteger()
        {
        }

        public SerializeValueInteger(int value) : base(value)
        {
        }
    }

    [Serializable]
    public class SerializeValue<T> : Value<T>
    {
        [SerializeField] private T value;

        public T Value { get => value; set => this.value = value; }

        public SerializeValue()
        {
            value = default;
        }

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

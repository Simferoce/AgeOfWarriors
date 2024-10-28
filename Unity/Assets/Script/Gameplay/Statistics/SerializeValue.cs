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

        public override T GetValue()
        {
            return value;
        }

        public override string GetDescription(Context context)
        {
            return $"<color=#000000>({value})</color>";
        }
    }
}

using System;

namespace Game
{
    [Serializable]
    public abstract class Adjustment
    {

    }

    [Serializable]
    public abstract class Adjustment<T> : Adjustment
    {
        public abstract T Adjust(T value);
    }
}

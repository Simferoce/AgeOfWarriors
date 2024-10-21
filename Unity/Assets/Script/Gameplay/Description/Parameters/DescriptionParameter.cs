using System;

namespace Game
{
    [Serializable]
    public abstract class DescriptionParameter
    {
        public abstract object GetValue(object context);
    }
}

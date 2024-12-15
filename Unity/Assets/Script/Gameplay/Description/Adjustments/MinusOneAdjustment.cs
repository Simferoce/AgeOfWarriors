using System;

namespace Game
{
    [Serializable]
    public class MinusOneAdjustment : Adjustment<float>, IFloatAdjustment
    {
        public override float Adjust(float value)
        {
            return value - 1;
        }
    }
}
using System;

namespace Game
{
    [Serializable]
    public class OneMinusAdjustment : Adjustment<float>, IFloatAdjustment
    {
        public override float Adjust(float value)
        {
            return 1 - value;
        }
    }
}
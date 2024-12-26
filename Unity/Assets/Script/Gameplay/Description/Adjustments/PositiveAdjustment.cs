using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class PositiveAdjustment : Adjustment<float>, IFloatAdjustment
    {
        public override float Adjust(float value)
        {
            return Mathf.Abs(value);
        }
    }
}

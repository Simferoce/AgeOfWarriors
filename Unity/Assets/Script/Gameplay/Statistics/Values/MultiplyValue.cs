﻿using System;
using UnityEngine;

namespace Game.Statistics
{
    [Serializable]
    public class MultiplyValue : Value<float>
    {
        [SerializeReference, SubclassSelector] private Value a;
        [SerializeReference, SubclassSelector] private Value b;

        public override void Initialize(StatisticRepository owner)
        {
            base.Initialize(owner);
            a.Initialize(owner);
            b.Initialize(owner);
        }

        public override float GetValue()
        {
            return a.GetValue<float>() * b.GetValue<float>();
        }
    }
}

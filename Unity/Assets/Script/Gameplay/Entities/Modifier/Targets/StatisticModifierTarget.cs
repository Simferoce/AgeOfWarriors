using Game.Statistics;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class StatisticModifierTarget : ModifierTarget
    {
        [SerializeField] private StatisticReference reference;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            reference.Initialize(modifier);
        }

        public override List<object> GetTargets()
        {
            return new List<object>() { reference.Get().Get<Entity>() };
        }
    }
}

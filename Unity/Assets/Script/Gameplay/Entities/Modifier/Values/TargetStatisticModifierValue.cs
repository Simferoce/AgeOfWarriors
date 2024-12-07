using Game.Statistics;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class TargetStatisticModifierValue : Value<float>
    {
        [SerializeField] private StatisticDefinition casterDefinition;
        [SerializeReference, SubclassSelector] private ModifierTarget target;

        public override void Initialize(StatisticRepository owner)
        {
            base.Initialize(owner);

            if (owner.Owner is not ModifierEntity modifierEntity)
            {
                Debug.LogError($"Excepting the type of {owner} to be of {nameof(ModifierEntity)}");
                return;
            }

            target.Initialize(modifierEntity);
        }

        public override float GetValue()
        {
            List<object> targets = target.GetTargets();
            return (targets[0] as Entity).GetCachedComponent<StatisticRepository>().GetOrThrow<float>(casterDefinition).Get<float>();
        }
    }
}

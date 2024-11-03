using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class StatisticModifierBehaviour : ModifierBehaviour
    {
        public enum StatisticModifierOperator
        {
            Flat,
            Percentage,
            Multiplier,
            Maximum,
            Minimum
        }

        [SerializeField] private StatisticModifierOperator modifierOperator;
        [SerializeField] private StatisticDefinition definition;
        [SerializeReference, SubclassSelector] private Value value;

        public StatisticModifierOperator Operator { get => modifierOperator; set => modifierOperator = value; }
        public StatisticDefinition Definition { get => definition; set => definition = value; }
        public Value Value { get => value; set => this.value = value; }

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            value.Initialize(modifier);
        }

        public T GetValue<T>()
        {
            return value.GetValue<T>();
        }
    }
}

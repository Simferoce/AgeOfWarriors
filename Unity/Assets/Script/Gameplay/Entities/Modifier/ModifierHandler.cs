using Game.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Modifier
{
    public class ModifierHandler : MonoBehaviour
    {
        public event Action<ModifierEntity> OnModifierRemoved;
        public event Action<ModifierEntity> OnModifierAdded;
        public Entity Entity { get; set; }

        private List<ModifierEntity> modifiers = new List<ModifierEntity>();

        private void Awake()
        {
            Entity = GetComponentInParent<Entity>();
        }

        public void Add(ModifierEntity modifier)
        {
            modifiers.Add(modifier);
            OnModifierAdded?.Invoke(modifier);
        }

        public void Remove(ModifierEntity modifier)
        {
            modifiers.Remove(modifier);
            OnModifierRemoved?.Invoke(modifier);
        }

        public float Modify(StatisticDefinition definition, float value)
        {
            List<StatisticModifierBehaviour> statisticModifierBehaviours = modifiers.SelectMany(x => x.Behaviours.OfType<StatisticModifierBehaviour>()).Where(x => x.Definition == definition).ToList();
            float flat = statisticModifierBehaviours.Where(x => x.Operator == StatisticModifierBehaviour.StatisticModifierOperator.Flat).Select(x => x.GetValue<float>()).Sum();
            float percentage = statisticModifierBehaviours.Where(x => x.Operator == StatisticModifierBehaviour.StatisticModifierOperator.Percentage).Select(x => x.GetValue<float>()).Sum();
            float muliplier = statisticModifierBehaviours.Where(x => x.Operator == StatisticModifierBehaviour.StatisticModifierOperator.Percentage).Select(x => x.GetValue<float>()).Aggregate(1f, (x, y) => x * y);

            return (value + flat) * (1 + percentage) * muliplier;
        }

        public List<ModifierEntity> GetModifiers()
        {
            return modifiers;
        }

        public ModifierEntity GetModifier(ModifierDefinition definition)
        {
            return modifiers.FirstOrDefault(x => x.Definition == definition);
        }

        public bool TryGetModifier(ModifierDefinition definition, ModifierApplier applier, out ModifierEntity modifier)
        {
            modifier = modifiers.FirstOrDefault(x => x.Definition == definition && x.Applier == applier);
            return modifier != null;
        }

        public bool TryGetModifier(ModifierDefinition definition, out ModifierEntity modifier)
        {
            modifier = modifiers.FirstOrDefault(x => x.Definition == definition);
            return modifier != null;
        }

        public bool TryGetUnique(ModifierDefinition definition, ModifierApplier applier, out ModifierEntity modifier)
        {
            UniqueType uniqueType = definition.GetUniqueType();

            if (uniqueType == UniqueType.None)
            {
                modifier = null;
                return false;
            }

            if (uniqueType == UniqueType.ByDefinition)
                return TryGetModifier(definition, out modifier);

            if (uniqueType == UniqueType.BySource)
                return TryGetModifier(definition, applier, out modifier);

            throw new NotImplementedException();
        }
    }
}

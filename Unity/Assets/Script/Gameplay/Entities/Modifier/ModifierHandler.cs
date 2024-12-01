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
        public Statistic this[StatisticDefinition definition] => statisticRepository[definition];

        private List<ModifierEntity> modifiers = new List<ModifierEntity>();
        private StatisticRepository statisticRepository = new StatisticRepository();

        private void Awake()
        {
            Entity = GetComponentInParent<Entity>();
            statisticRepository.Initialize(this);

            StatisticFloat flatDefense = new StatisticFloat("flat_defense", StatisticDefinitionRegistry.Instance.FlatDefense, 0f, (float baseValue) => baseValue + modifiers.Sum(StatisticDefinitionRegistry.Instance.FlatDefense));
            StatisticFloat multiplierDamage = new StatisticFloat("multiplier_damage", StatisticDefinitionRegistry.Instance.MultiplierDamage, 1f, (float baseValue) => baseValue * modifiers.Multiply(StatisticDefinitionRegistry.Instance.MultiplierDamage));
            StatisticBool stagger = new StatisticBool("stagger", StatisticDefinitionRegistry.Instance.Stagger, false, (bool baseValue) => baseValue || modifiers.Union(StatisticDefinitionRegistry.Instance.Stagger));

            statisticRepository.Add(flatDefense);
            statisticRepository.Add(multiplierDamage);
            statisticRepository.Add(stagger);
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

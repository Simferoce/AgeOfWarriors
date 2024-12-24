using Game.Statistics;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Modifier
{
    public class ModifierApplier : MonoBehaviour
    {
        public delegate void OnModifierRemovedDelegate(ModifierEntity modifier);
        public delegate void OnModifierAppliedDelegate(ModifierEntity modifier);
        public delegate void OnChildModifierAppliedDelegate(ModifierEntity modifier);
        public event OnModifierAppliedDelegate OnModifierApplied;
        public event OnChildModifierAppliedDelegate OnChildModifierApplied;
        public event OnModifierRemovedDelegate OnModifierRemoved;

        public Entity Entity { get; set; }
        public IReadOnlyList<ModifierEntity> CurrentlyAppliedModifiers => currentlyAppliedModifiers;

        private readonly List<ModifierEntity> currentlyAppliedModifiers = new List<ModifierEntity>();

        private void Awake()
        {
            Entity = GetComponentInParent<Entity>();
        }

        private void OnDestroy()
        {
            foreach (ModifierEntity modifier in CurrentlyAppliedModifiers)
                modifier.OnRemoved -= OnRemoved;

            currentlyAppliedModifiers.Clear();
        }

        public void Apply(ModifierDefinition definition, ModifierHandler target, params ModifierParameter[] parameters)
        {
            if (target.TryGetUnique(definition, this, out ModifierEntity modifier))
            {
                modifier.Refresh(parameters);
            }
            else
            {
                modifier = definition.Instantiate();
                StatisticRepository modifierStatisticRepository = modifier.GetCachedComponent<StatisticRepository>();

                currentlyAppliedModifiers.Add(modifier);
                modifier.Initialize(target, this, parameters);
                target.Add(modifier);

                modifier.OnRemoved += OnRemoved;
            }

            OnModifierApplied?.Invoke(modifier);
            NotifyApplied(Entity, modifier);
        }

        public void NotifyApplied(Entity entity, ModifierEntity modifier)
        {
            if (entity.TryGetCachedComponent<ModifierApplier>(out ModifierApplier modifierApplier))
                modifierApplier.OnChildModifierApplied?.Invoke(modifier);

            if (entity.Parent != null)
                NotifyApplied(entity.Parent, modifier);
        }

        public void OnRemoved(ModifierEntity modifier)
        {
            modifier.OnRemoved -= OnRemoved;
            currentlyAppliedModifiers.Remove(modifier);
            OnModifierRemoved?.Invoke(modifier);
        }
    }
}

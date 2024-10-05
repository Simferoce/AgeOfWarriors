using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ModifierApplier : MonoBehaviour, IComponent
    {
        public delegate void OnModifierRemovedDelegate(Modifier modifier);
        public delegate void OnModifierAppliedDelegate(Modifier modifier);
        public event OnModifierAppliedDelegate OnModifierApplied;
        public event OnModifierRemovedDelegate OnModifierRemoved;

        public Entity Entity { get; set; }
        public IReadOnlyList<Modifier> CurrentlyAppliedModifiers => currentlyAppliedModifiers;

        private readonly List<Modifier> currentlyAppliedModifiers = new List<Modifier>();

        private void Awake()
        {
            Entity = GetComponentInParent<Entity>();
        }

        private void OnDestroy()
        {
            foreach (Modifier modifier in CurrentlyAppliedModifiers)
                modifier.OnRemoved -= OnRemoved;

            currentlyAppliedModifiers.Clear();
        }

        public void Apply(Modifier modifier, ModifierHandler target, params ModifierParameter[] parameters)
        {
            currentlyAppliedModifiers.Add(modifier);
            modifier.Initialize(target, this, parameters);
            target.Add(modifier);

            modifier.OnRemoved += OnRemoved;
            OnModifierApplied?.Invoke(modifier);
        }

        public void OnRemoved(Modifier modifier)
        {
            modifier.OnRemoved -= OnRemoved;
            currentlyAppliedModifiers.Remove(modifier);
            OnModifierRemoved?.Invoke(modifier);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class ModifierHandler : MonoBehaviour
    {
        public event Action<Modifier> OnModifierRemoved;
        public event Action<Modifier> OnModifierAdded;
        public Entity Entity { get; set; }

        private List<Modifier> modifiers = new List<Modifier>();

        private void Awake()
        {
            Entity = GetComponentInParent<Entity>();
        }

        public void Add(Modifier modifier)
        {
            Entity.GetCachedComponent<StatisticIndex>().Add(modifier.GetCachedComponent<StatisticIndex>());
            modifiers.Add(modifier);
            OnModifierAdded?.Invoke(modifier);
        }

        public void Remove(Modifier modifier)
        {
            Entity.GetCachedComponent<StatisticIndex>().Remove(modifier.GetCachedComponent<StatisticIndex>());
            modifiers.Remove(modifier);
            OnModifierRemoved?.Invoke(modifier);
        }

        public List<Modifier> GetModifiers()
        {
            return modifiers;
        }

        public bool TryGetModifier(ModifierDefinition definition, out Modifier modifier)
        {
            modifier = modifiers.FirstOrDefault(x => x.Definition == definition);
            return modifier != null;
        }
    }
}

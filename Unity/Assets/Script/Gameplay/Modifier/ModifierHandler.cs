using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public class ModifierHandler : Entity, IModifiable
    {
        public event Action<Modifier> ModifierRemoved;

        private List<Modifier> modifiers = new List<Modifier>();

        public void AddModifier(Modifier modifier)
        {
            modifier.Initialize();
            modifiers.Add(modifier);
        }

        public List<Modifier> GetModifiers()
        {
            return modifiers;
        }

        private void Update()
        {
            foreach (Modifier modifier in modifiers.ToList())
            {
                modifier.Update();
            }
        }

        public void RemoveModifier(Modifier modifier)
        {
            modifier.Dispose();
            modifiers.Remove(modifier);
            ModifierRemoved?.Invoke(modifier);
        }

        private void OnDestroy()
        {
            Clear();
        }

        public bool TryGetModifier(ModifierDefinition definition, out Modifier modifier)
        {
            modifier = modifiers.FirstOrDefault(x => x.Definition == definition);
            return modifier != null;
        }

        public void Clear()
        {
            foreach (Modifier modifier in modifiers.ToList())
            {
                RemoveModifier(modifier);
            }
        }
    }
}

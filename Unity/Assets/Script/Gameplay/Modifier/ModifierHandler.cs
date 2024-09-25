using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class ModifierHandler : MonoBehaviour, IComponent
    {
        public event Action<Modifier> OnModifierRemoved;
        public event Action<Modifier> OnModifierAdded;
        public Entity Entity { get; set; }

        private List<Modifier> modifiers = new List<Modifier>();

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

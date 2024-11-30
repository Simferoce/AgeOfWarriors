using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ModifierApplier : MonoBehaviour
    {
        public event System.Action<Modifier> OnModifierAdded;

        public List<Modifier> AppliedModifiers { get; set; } = new List<Modifier>();
        public Entity Entity { get; private set; }

        private void Awake()
        {
            Entity = GetComponent<Entity>();
        }

        public void Apply(ModifierHandler target, Modifier modifier, List<ModifierParameter> parameters)
        {
            modifier.Initialize(target, this, parameters);
            target.AddModifier(modifier);
            AppliedModifiers.Add(modifier);

            OnModifierAdded?.Invoke(modifier);
            modifier.OnDispose += ModifierOnDispose;
        }

        private void ModifierOnDispose(Modifier modifier)
        {
            modifier.OnDispose -= ModifierOnDispose;
            AppliedModifiers.Remove(modifier);
        }
    }
}

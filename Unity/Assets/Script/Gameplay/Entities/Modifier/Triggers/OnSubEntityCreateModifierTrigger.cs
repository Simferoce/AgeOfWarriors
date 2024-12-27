using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class OnSubEntityCreateModifierTrigger : ModifierTrigger, IModifierTargetProvider
    {
        [SerializeReference, SubclassSelector] private ModifierTargetFilter targetFilter;

        private Entity target;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);

            modifier.Target.Entity.OnDescendantAdded += Entity_OnDescendantAdded;
        }

        private void Entity_OnDescendantAdded(Entity entity)
        {
            if (targetFilter.Execute(entity))
            {
                target = entity;
                Trigger();
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            modifier.Target.Entity.OnDescendantAdded -= Entity_OnDescendantAdded;
        }

        public List<object> GetTargets()
        {
            return new List<object>() { target };
        }
    }
}

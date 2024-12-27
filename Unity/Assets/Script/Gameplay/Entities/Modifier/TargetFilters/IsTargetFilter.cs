using System;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class IsTargetFilter : ModifierTargetFilter
    {
        [SerializeReference, SubclassSelector] private ModifierTarget modifierTarget;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            modifierTarget.Initialize(modifier);
        }

        public override bool Execute(Entity target)
        {
            return (Entity)modifierTarget.GetTargets()[0] == target;
        }
    }
}

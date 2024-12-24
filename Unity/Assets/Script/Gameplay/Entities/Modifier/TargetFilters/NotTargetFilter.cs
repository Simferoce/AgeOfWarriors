using System;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class NotTargetFilter : ModifierTargetFilter
    {
        [SerializeReference, SubclassSelector] private ModifierTargetFilter targetFilter;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            targetFilter.Initialize(modifier);
        }

        public override bool Execute(Entity target)
        {
            return !targetFilter.Execute(target);
        }
    }
}
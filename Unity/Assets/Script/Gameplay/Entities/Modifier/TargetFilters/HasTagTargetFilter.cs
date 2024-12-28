using System;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class HasTagTargetFilter : ModifierTargetFilter
    {
        [SerializeField] private Entity.EntityTag tag;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
        }

        public override bool Execute(Entity target)
        {
            return target.Tags.Contains(tag);
        }
    }
}
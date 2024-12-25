using System;
using System.Linq;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class SetStackBaseOnDurationActiveModifierBehaviour : ModifierBehaviour
    {
        private StackModifierBehaviour stackModifierBehaviour;
        private float startedAt;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            startedAt = Time.time;
            stackModifierBehaviour = modifier.Behaviours.OfType<StackModifierBehaviour>().FirstOrDefault();
        }

        public override Result Update()
        {
            stackModifierBehaviour.SetStack((int)(Time.time - startedAt));
            return base.Update();
        }
    }
}

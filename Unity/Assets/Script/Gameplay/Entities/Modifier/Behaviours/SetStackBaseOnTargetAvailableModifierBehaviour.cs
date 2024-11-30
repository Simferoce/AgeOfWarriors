using System;
using System.Linq;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class SetStackBaseOnTargetAvailableModifierBehaviour : ModifierBehaviour
    {
        [SerializeReference, SubclassSelector] private ModifierTarget target;

        private StackModifierBehaviour stackModifierBehaviour;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            stackModifierBehaviour = modifier.Behaviours.OfType<StackModifierBehaviour>().FirstOrDefault();
            target.Initialize(modifier);
        }

        public override Result Update()
        {
            stackModifierBehaviour.SetStack(target.GetTargets().Count);
            return base.Update();
        }
    }
}

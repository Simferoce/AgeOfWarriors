using System;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class PeriodicTimeModifierTrigger : ModifierTrigger
    {
        private StackModifierBehaviour stackModifierBehaviour;
        private float lastPeriod;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            lastPeriod = Time.time;
        }

        public override void Update()
        {
            base.Update();

            if (Time.time - lastPeriod >= 1)
            {
                Trigger();
                lastPeriod = Time.time;
            }
        }
    }
}

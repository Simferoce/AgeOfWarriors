using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class TriggerModifierBehaviour : ModifierBehaviour, IModifierTriggerProvider
    {
        [SerializeReference, SubclassSelector] private List<ModifierTrigger> triggers;
        [SerializeReference, SubclassSelector] private List<ModifierEffect> effects;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);

            foreach (ModifierTrigger trigger in triggers)
            {
                trigger.Initialize(modifier);
                trigger.OnTrigger += Trigger_OnTrigger;
            }

            foreach (ModifierEffect effect in effects)
                effect.Initialize(modifier);
        }

        public override Result Update()
        {
            foreach (ModifierTrigger trigger in triggers)
                trigger.Update();

            return base.Update();
        }

        public override void Dispose()
        {
            base.Dispose();

            foreach (ModifierTrigger trigger in triggers)
                trigger.OnTrigger -= Trigger_OnTrigger;
        }

        private void Trigger_OnTrigger(ModifierTrigger trigger)
        {
            foreach (ModifierEffect effect in effects)
                effect.Execute();
        }

        public List<ModifierTrigger> GetModifierTriggers()
        {
            return triggers;
        }
    }
}
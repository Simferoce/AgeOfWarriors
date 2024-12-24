using Game.Components;
using System;
using System.Collections.Generic;

namespace Game.Modifier
{
    [Serializable]
    public class OnTakeDownModifierTrigger : ModifierTrigger, IModifierTargetProvider
    {
        private Entity takeDown;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            modifier.Target.Entity.GetCachedComponent<AttackFactory>().OnTakeDown += OnTakeDownModifierTrigger_OnTakeDown;
        }

        private void OnTakeDownModifierTrigger_OnTakeDown(AttackResult result)
        {
            takeDown = result.Target.Entity;
            Trigger();
        }

        public override void Dispose()
        {
            base.Dispose();
            modifier.Target.Entity.GetCachedComponent<AttackFactory>().OnTakeDown -= OnTakeDownModifierTrigger_OnTakeDown;
        }

        public List<object> GetTargets()
        {
            return new List<object>() { takeDown };
        }
    }
}

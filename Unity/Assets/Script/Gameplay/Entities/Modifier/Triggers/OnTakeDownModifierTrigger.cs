using Game.Components;
using System;

namespace Game.Modifier
{
    [Serializable]
    public class OnTakeDownModifierTrigger : ModifierTrigger
    {
        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            modifier.Target.Entity.GetCachedComponent<AttackFactory>().OnTakeDown += OnTakeDownModifierTrigger_OnTakeDown;
        }

        private void OnTakeDownModifierTrigger_OnTakeDown(AttackResult result)
        {
            Trigger();
        }

        public override void Dispose()
        {
            base.Dispose();
            modifier.Target.Entity.GetCachedComponent<AttackFactory>().OnTakeDown -= OnTakeDownModifierTrigger_OnTakeDown;
        }
    }
}

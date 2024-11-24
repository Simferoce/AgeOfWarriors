using Game.Components;
using System;

namespace Game.Modifier
{
    [Serializable]
    public class OnAttackLandedModifierTrigger : ModifierTrigger
    {
        private AttackFactory attackFactory;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            attackFactory = modifier.Target.Entity.GetCachedComponent<AttackFactory>();
            attackFactory.OnAttackLanded += AttackFactory_OnAttackLanded;
        }

        private void AttackFactory_OnAttackLanded(AttackResult attackResult)
        {
            Trigger();
        }

        public override void Dispose()
        {
            base.Dispose();
            attackFactory.OnAttackLanded -= AttackFactory_OnAttackLanded;
        }
    }
}

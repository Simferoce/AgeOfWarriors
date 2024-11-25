using Game.Components;
using System;

namespace Game.Modifier
{
    [Serializable]
    public class OnAttackLandedModifierTriggerKillingBlowFilter : OnAttackLandedModifierTriggerFilter
    {
        public override bool Execute(AttackResult attackResult)
        {
            return attackResult.KillingBlow;
        }
    }
}

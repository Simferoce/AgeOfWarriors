using Game.Components;
using System;

namespace Game.Modifier
{
    [Serializable]
    public class EnemyOnHitModifierTriggerCondition : OnHitModifierTriggerCondition
    {
        public override bool Execute(AttackResult result, Attackable receiver)
        {
            return result.AttackData.Source.Entity["faction"].Get<FactionType>() != modifier.Target.Entity["faction"].Get<FactionType>();
        }
    }
}

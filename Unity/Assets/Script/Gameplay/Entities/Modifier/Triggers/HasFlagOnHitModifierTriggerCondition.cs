using Game.Components;
using System;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class HasFlagOnHitModifierTriggerCondition : OnHitModifierTriggerCondition
    {
        [SerializeField] private AttackData.Flag flags;

        public override bool Execute(AttackResult result, Attackable receiver)
        {
            return result.AttackData.Flags.HasFlag(flags);
        }
    }
}

using Game.Components;
using System;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class OnAttackLandedModifierTriggerFlagFilter : OnAttackLandedModifierTriggerFilter
    {
        [SerializeField] private AttackData.Flag flag;

        public override bool Execute(AttackResult attackResult)
        {
            return attackResult.AttackData.Flags.HasFlag(flag);
        }
    }
}

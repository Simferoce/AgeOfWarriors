using Game.Components;
using System;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class InverseOnHitModifierTriggerCondition : OnHitModifierTriggerCondition
    {
        [SerializeReference, SubclassSelector] private OnHitModifierTriggerCondition condition;

        public override bool Execute(AttackResult result, Attackable receiver)
        {
            return !condition.Execute(result, receiver);
        }
    }
}

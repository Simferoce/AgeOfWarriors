using Game.Components;
using System;

namespace Game.Modifier
{
    [Serializable]
    public abstract class OnAttackLandedModifierTriggerFilter
    {
        public abstract bool Execute(AttackResult attackResult);
    }
}

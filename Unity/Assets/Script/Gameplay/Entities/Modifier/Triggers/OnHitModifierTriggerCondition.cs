using Game.Components;
using System;

namespace Game.Modifier
{
    [Serializable]
    public abstract class OnHitModifierTriggerCondition
    {
        protected ModifierEntity modifier;

        public virtual void Initialize(ModifierEntity modifier)
        {
            this.modifier = modifier;
        }

        public abstract bool Execute(AttackResult result, Attackable receiver);
    }
}

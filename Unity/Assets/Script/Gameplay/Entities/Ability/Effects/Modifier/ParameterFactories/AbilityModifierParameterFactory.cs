using Game.Modifier;
using System;

namespace Game.Ability
{
    [Serializable]
    public abstract class AbilityModifierParameterFactory
    {
        public abstract ModifierParameter Create(AbilityEntity ability);
    }
}

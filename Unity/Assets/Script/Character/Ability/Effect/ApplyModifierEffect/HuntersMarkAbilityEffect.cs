using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class HuntersMarkAbilityEffect : AbilityEffect
    {
        [SerializeField] private float damage;
        [SerializeField] private float duration;
        [SerializeField] private HunterMarkModifierDefinition hunterMarkModifierDefinition;

        public override void Apply()
        {
            List<IAttackable> attackable = Ability.Targets;
            IModifiable modifiable = attackable.FirstOrDefault(x => x is IModifiable) as IModifiable;

            if (modifiable != null)
            {
                HunterMarkModifierDefinition.Modifier huntersMark = (HunterMarkModifierDefinition.Modifier)modifiable.GetModifiers().FirstOrDefault(x => x is HunterMarkModifierDefinition.Modifier huntersMark && huntersMark.Source == this);

                if (huntersMark != null)
                {
                    huntersMark.Refresh();
                }
                else
                {
                    huntersMark = new HunterMarkModifierDefinition.Modifier(modifiable, hunterMarkModifierDefinition, this, damage, modifiable as IAttackable)
                        .With(new CharacterModifierTimeElement(duration));

                    modifiable.AddModifier(huntersMark);
                }
            }
        }
    }
}

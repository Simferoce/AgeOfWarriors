﻿using System;
using System.Linq;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class HuntersMarkAbilityEffect : AbilityEffect
    {
        [SerializeField] private StatisticReference<float> damage;
        [SerializeField] private StatisticReference<float> duration;
        [SerializeField] private HunterMarkModifierDefinition hunterMarkModifierDefinition;

        public override void Apply()
        {
            IModifiable modifiable = Ability.Targets.OfType<IModifiable>().FirstOrDefault();

            if (modifiable != null)
            {
                HunterMarkModifierDefinition.Modifier huntersMark = (HunterMarkModifierDefinition.Modifier)modifiable.GetModifiers().FirstOrDefault(x => x is HunterMarkModifierDefinition.Modifier huntersMark && huntersMark.Source == this);

                if (huntersMark != null)
                {
                    huntersMark.Refresh();
                }
                else
                {
                    huntersMark = new HunterMarkModifierDefinition.Modifier(modifiable, hunterMarkModifierDefinition, this, damage.GetValueOrThrow(Ability), modifiable as IAttackable)
                        .With(new CharacterModifierTimeElement(duration.GetValueOrThrow(Ability)));

                    modifiable.AddModifier(huntersMark);
                }
            }
        }
    }
}
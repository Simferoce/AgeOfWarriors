using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class InspiringPresenceAbilityEffect : AbilityEffect, ILingeringAbilityEffect
    {
        [SerializeField] private StatisticReference<float> duration;
        [SerializeField] private StatisticReference<float> area;
        [SerializeField] private StatisticReference<float> buffDuration;
        [SerializeField] private StatisticReference<float> defense;
        [SerializeField] private InspiringPresenceModifierDefinition inspiringPresenceModifierDefinition;

        private float? startedAt = null;

        public override void Apply()
        {
            startedAt = Time.time;
        }

        public bool Update(Character character)
        {
            if (startedAt == null)
                return false;

            if (character.IsDead)
                return true;

            if (Time.time - startedAt > duration.GetValueOrThrow(Ability))
            {
                return true;
            }

            List<Character> characters = AgentObject.All.OfType<Character>()
                .Where(x => x.Faction == character.Faction
                    && Mathf.Abs(character.transform.position.x - x.transform.position.x) < area.GetValueOrThrow(Ability))
                .ToList();

            foreach (Character characterToBuff in characters)
            {
                InspiringPresenceModifierDefinition.Modifier inspiringPresenceBuff = (InspiringPresenceModifierDefinition.Modifier)characterToBuff.GetModifiers().FirstOrDefault(x => x is InspiringPresenceModifierDefinition.Modifier);
                if (inspiringPresenceBuff != null)
                {
                    if (inspiringPresenceBuff.Source == this)
                    {
                        inspiringPresenceBuff.Refresh();
                    }
                }
                else
                {
                    inspiringPresenceBuff = new InspiringPresenceModifierDefinition.Modifier(character, inspiringPresenceModifierDefinition, defense.GetValueOrThrow(Ability), this)
                        .With(new CharacterModifierTimeElement(buffDuration.GetValueOrThrow(Ability)));

                    characterToBuff.AddModifier(inspiringPresenceBuff);
                }
            }

            return false;
        }

        public override void OnAbilityEnded()
        {
            startedAt = null;
        }
    }
}

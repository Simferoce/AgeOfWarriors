using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class InspiringPresenceAbilityEffect : AbilityEffect
    {
        [SerializeField] private StatisticReference<float> area;
        [SerializeField] private StatisticReference<float> buffDuration;
        [SerializeField] private StatisticReference<float> defense;
        [SerializeField] private DefenseModifierDefinition inspiringPresenceModifierDefinition;

        private float? startedAt = null;

        public override void Apply()
        {
            startedAt = Time.time;

            List<Character> characters = AgentObject.All.OfType<Character>()
               .Where(x => x.Faction == Ability.Character.Faction
                   && Mathf.Abs(Ability.Character.transform.position.x - x.transform.position.x) < area.GetValueOrThrow(Ability))
               .ToList();

            foreach (Character characterToBuff in characters)
            {
                DefenseModifierDefinition.Modifier inspiringPresenceBuff = (DefenseModifierDefinition.Modifier)characterToBuff
                    .GetCachedComponent<IModifiable>().GetModifiers().FirstOrDefault(x => x is DefenseModifierDefinition.Modifier);

                if (inspiringPresenceBuff != null)
                {
                    if (inspiringPresenceBuff.Source == this)
                    {
                        inspiringPresenceBuff.Refresh();
                    }
                }
                else
                {
                    inspiringPresenceBuff = new DefenseModifierDefinition.Modifier(Ability.Character, inspiringPresenceModifierDefinition, defense.GetValueOrThrow(Ability), this)
                        .With(new CharacterModifierTimeElement(buffDuration.GetValueOrThrow(Ability)));

                    characterToBuff.GetCachedComponent<IModifiable>().AddModifier(inspiringPresenceBuff);
                }
            }
        }

        public override void OnAbilityEnded()
        {
            startedAt = null;
        }
    }
}

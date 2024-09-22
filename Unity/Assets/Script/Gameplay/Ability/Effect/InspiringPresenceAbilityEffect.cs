using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class InspiringPresenceAbilityEffect : AbilityEffect
    {
        [SerializeField] private StatisticReference area;
        [SerializeField] private StatisticReference buffDuration;
        [SerializeField] private StatisticReference defense;
        [SerializeField] private DefenseModifierDefinition inspiringPresenceModifierDefinition;

        private float? startedAt = null;

        public override void Initialize(Ability ability)
        {
            base.Initialize(ability);
            area.Initialize(ability);
            buffDuration.Initialize(ability);
            defense.Initialize(ability);
        }

        public override void Apply()
        {
            startedAt = Time.time;

            List<Character> characters = AgentObject.All.OfType<Character>()
               .Where(x => x.Faction == (x.Entity as AgentObject).Faction
                   && Mathf.Abs((x.Entity as AgentObject).transform.position.x - x.transform.position.x) < area)
               .ToList();

            foreach (Character characterToBuff in characters)
            {
                DefenseModifierDefinition.Modifier inspiringPresenceBuff = (DefenseModifierDefinition.Modifier)characterToBuff
                    .Entity.GetCachedComponent<ModifierHandler>().GetModifiers().FirstOrDefault(x => x is DefenseModifierDefinition.Modifier);

                if (inspiringPresenceBuff != null)
                {
                    if (inspiringPresenceBuff.Source == Ability.Caster.Entity.GetCachedComponent<IModifierSource>())
                    {
                        inspiringPresenceBuff.Refresh();
                    }
                }
                else
                {
                    inspiringPresenceBuff = new DefenseModifierDefinition.Modifier(
                            Ability.Caster.Entity.GetCachedComponent<Character>(),
                            inspiringPresenceModifierDefinition,
                            defense,
                            Ability.Caster.Entity.GetCachedComponent<IModifierSource>())
                        .With(new CharacterModifierTimeElement(buffDuration));

                    characterToBuff.Entity.GetCachedComponent<ModifierHandler>().AddModifier(inspiringPresenceBuff);
                }
            }
        }

        public override void OnAbilityEnded()
        {
            startedAt = null;
        }
    }
}

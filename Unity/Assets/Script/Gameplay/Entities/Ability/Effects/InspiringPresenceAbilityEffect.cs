using Game.Character;
using Game.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Ability
{
    [Serializable]
    public class InspiringPresenceAbilityEffect : AbilityEffect
    {
        [SerializeField] private Statistic area;
        [SerializeField] private Statistic buffDuration;
        [SerializeField] private Statistic defense;
        //[SerializeField] private DefenseModifierDefinition inspiringPresenceModifierDefinition;

        private float? startedAt = null;

        public override void Initialize(AbilityEntity ability)
        {
            base.Initialize(ability);
        }

        public override bool Validate()
        {
            bool changed = base.Validate();
            if (area != null && area.Definition != StatisticDefinitionRepository.Instance.GetById(StatisticIdentifiant.Range))
            {
                area.Definition = StatisticDefinitionRepository.Instance.GetById(StatisticIdentifiant.Range);
                changed = true;
            }
            if (buffDuration != null && buffDuration.Definition != StatisticDefinitionRepository.Instance.GetById(StatisticIdentifiant.BuffDuration))
            {
                buffDuration.Definition = StatisticDefinitionRepository.Instance.GetById(StatisticIdentifiant.BuffDuration);
                changed = true;
            }
            if (defense != null && defense.Definition != StatisticDefinitionRepository.Instance.GetById(StatisticIdentifiant.DefenseFlat))
            {
                defense.Definition = StatisticDefinitionRepository.Instance.GetById(StatisticIdentifiant.DefenseFlat);
                changed = true;
            }

            return changed;
        }

        public override void Apply()
        {
            startedAt = Time.time;

            List<CharacterEntity> characters = EntityRepository.Instance.GetByType<CharacterEntity>()
               .Where(x => x.Faction == x.Faction
                   && Mathf.Abs(x.transform.position.x - x.transform.position.x) < area.GetValue<float>(Ability))
               .ToList();

            //foreach (Character characterToBuff in characters)
            //{
            //    DefenseModifierDefinition.Modifier inspiringPresenceBuff = (DefenseModifierDefinition.Modifier)characterToBuff
            //        .Entity.GetCachedComponent<ModifierHandler>().GetModifiers().FirstOrDefault(x => x is DefenseModifierDefinition.Modifier);

            //    if (inspiringPresenceBuff != null)
            //    {
            //        if (inspiringPresenceBuff.Source == Ability.Caster.Entity.GetCachedComponent<IModifierSource>())
            //        {
            //            inspiringPresenceBuff.Refresh();
            //        }
            //    }
            //    else
            //    {
            //        inspiringPresenceBuff = new DefenseModifierDefinition.Modifier(
            //                Ability.Caster.Entity.GetCachedComponent<Character>(),
            //                inspiringPresenceModifierDefinition,
            //                defense,
            //                Ability.Caster.Entity.GetCachedComponent<IModifierSource>())
            //            .With(new TimeModifierBehaviour(buffDuration));

            //        characterToBuff.Entity.GetCachedComponent<ModifierHandler>().AddModifier(inspiringPresenceBuff);
            //    }
            //}
        }

        public override void OnAbilityEnded()
        {
            startedAt = null;
        }
    }
}

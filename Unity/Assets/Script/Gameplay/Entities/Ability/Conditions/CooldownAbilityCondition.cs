using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Ability
{
    [Serializable]
    public class CooldownAbilityCondition : AbilityCondition
    {
        [SerializeReference, SubclassSelector] private Statistic cooldown;

        public float Cooldown => cooldown.GetValue<float>(ability);

        private float lastUsed = 0f;

        public override void Initialize(AbilityEntity ability)
        {
            base.Initialize(ability);
            lastUsed = float.MinValue;

            if (cooldown != null)
                ability.GetCachedComponent<StatisticIndex>().Add(new StatisticFunction<float>(() => Cooldown, cooldown.Name, StatisticDefinitionRepository.Instance.GetById(StatisticIdentifiant.Cooldown)));
        }

        public override bool Validate()
        {
            bool changed = base.Validate();
            if (cooldown != null && cooldown.Definition != StatisticDefinitionRepository.Instance.GetById(StatisticIdentifiant.Cooldown))
            {
                cooldown.Definition = StatisticDefinitionRepository.Instance.GetById(StatisticIdentifiant.Cooldown);
                changed = true;
            }

            return changed;
        }

        public override bool Execute()
        {
            return Time.time - lastUsed > Cooldown;
        }

        public override void OnAbilityEnded()
        {
            lastUsed = Time.time;
        }
    }
}

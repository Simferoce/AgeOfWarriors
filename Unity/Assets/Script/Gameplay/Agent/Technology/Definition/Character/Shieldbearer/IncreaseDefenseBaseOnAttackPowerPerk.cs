using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseDefenseBaseOnAttackPowerPerk", menuName = "Definition/Technology/Shieldbearer/IncreaseDefenseBaseOnAttackPowerPerk")]
    public class IncreaseDefenseBaseOnAttackPowerPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseDefenseBaseOnAttackPowerPerk>
        {
            public Modifier(ModifierHandler modifiable, IncreaseDefenseBaseOnAttackPowerPerk modifierDefinition, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
            }

            public override IEnumerable<Statistic> GetStatistic()
            {
                yield return new StatisticTemporary<float>(this, "", Mathf.Floor(modifiable.Entity.GetCachedComponent<Character>().Defense / definition.defenseRequired) * definition.attackPowerIncrease, StatisticRepository.GetDefinition(StatisticRepository.AttackPower));

                foreach (Statistic statistic in base.GetStatistic())
                    yield return statistic;
            }
        }

        [SerializeField] private float attackPowerIncrease;
        [SerializeField] private float defenseRequired;

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }
    }
}

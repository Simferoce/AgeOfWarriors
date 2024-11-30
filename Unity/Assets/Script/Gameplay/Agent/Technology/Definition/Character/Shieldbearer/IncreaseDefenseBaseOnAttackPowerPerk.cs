using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseDefenseBaseOnAttackPowerPerk", menuName = "Definition/Technology/Shieldbearer/IncreaseDefenseBaseOnAttackPowerPerk")]
    public class IncreaseDefenseBaseOnAttackPowerPerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseDefenseBaseOnAttackPowerPerk>
        {
            private Statistic<float> attackPowerFlat;

            public Modifier(IncreaseDefenseBaseOnAttackPowerPerk modifierDefinition) : base(modifierDefinition)
            {
                attackPowerFlat = new Statistic<float>(StatisticDefinition.FlatAttackPower, 0f);
                StatisticRegistry.Register(attackPowerFlat);
            }

            public override void Update()
            {
                base.Update();
                attackPowerFlat.SetValue(Mathf.Floor(modifiable.Entity.GetCachedComponent<Character>().Defense / definition.defenseRequired) * definition.attackPowerIncrease);
            }

            public override void Dispose()
            {
                base.Dispose();
                StatisticRegistry.Unregister(attackPowerFlat);
            }
        }

        [SerializeField] private float attackPowerIncrease;
        [SerializeField] private float defenseRequired;

        public override string ParseDescription()
        {
            return string.Format(Description, attackPowerIncrease, defenseRequired);
        }

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }
    }
}

using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseAttackPowerBaseOnRemainingHealthPerk", menuName = "Definition/Technology/Berserker/IncreaseAttackPowerBaseOnRemainingHealthPerk")]
    public class IncreaseAttackPowerBaseOnRemainingHealthPerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseAttackPowerBaseOnRemainingHealthPerk>
        {
            private Statistic<float> attackPowerFlat;

            public Modifier(IncreaseAttackPowerBaseOnRemainingHealthPerk modifierDefinition) : base(modifierDefinition)
            {
                attackPowerFlat = new Statistic<float>(StatisticDefinition.FlatAttackPower);
                StatisticRegistry.Register(attackPowerFlat);
            }

            public override void Update()
            {
                base.Update();
                attackPowerFlat.SetValue(modifiable.Entity.TryGetCachedComponent<Character>(out Character character)
                    && character.Health / character.MaxHealth < definition.attackpower
                    ? definition.threshold
                    : 0f);
            }

            public override void Dispose()
            {
                base.Dispose();
                StatisticRegistry.Unregister(attackPowerFlat);
            }
        }

        [SerializeField] private float attackpower;
        [SerializeField] private float threshold;

        public override string ParseDescription()
        {
            return string.Format(Description, attackpower, threshold);
        }

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }
    }
}

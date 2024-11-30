using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "DamageAgainstWeakPerk", menuName = "Definition/Technology/Shieldbearer/DamageAgainstWeakPerk")]
    public class DamageAgainstWeakPerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, DamageAgainstWeakPerk>
        {
            private Statistic<float> damageDealtAgainstWeak;

            public Modifier(DamageAgainstWeakPerk modifierDefinition) : base(modifierDefinition)
            {
                damageDealtAgainstWeak = new Statistic<float>(StatisticDefinition.PercentageDamageDealtAgainstWeak, definition.damageDealtAgainstWeak);
                StatisticRegistry.Register(damageDealtAgainstWeak);
            }

            public override void Dispose()
            {
                base.Dispose();
                StatisticRegistry.Unregister(damageDealtAgainstWeak);
            }
        }

        [SerializeField] private float damageDealtAgainstWeak;

        public override string ParseDescription()
        {
            return string.Format(Description, damageDealtAgainstWeak);
        }

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }
    }
}

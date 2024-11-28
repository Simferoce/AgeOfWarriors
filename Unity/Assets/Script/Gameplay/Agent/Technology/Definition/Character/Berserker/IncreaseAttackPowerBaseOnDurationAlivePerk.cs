using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseAttackPowerBaseOnDurationAlivePerk", menuName = "Definition/Technology/Berserker/IncreaseAttackPowerBaseOnDurationAlivePerk")]
    public class IncreaseAttackPowerBaseOnDurationAlivePerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseAttackPowerBaseOnDurationAlivePerk>
        {
            private float startedAt;
            private Statistic<float> attackPowerFlat;

            public Modifier(ModifierHandler modifiable, IncreaseAttackPowerBaseOnDurationAlivePerk modifierDefinition, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
                startedAt = Time.time;
                attackPowerFlat = new Statistic<float>(StatisticDefinition.AttackPowerFlat);
                StatisticRegistry.Register(attackPowerFlat);
            }

            public override void Update()
            {
                base.Update();
                attackPowerFlat.SetValue(definition.attackpowerPerSeconds * (Time.time - startedAt));
            }

            public override void Dispose()
            {
                base.Dispose();
                StatisticRegistry.Unregister(attackPowerFlat);
            }
        }

        [SerializeField] private float attackpowerPerSeconds;

        public float AttackPower(Modifier modifier) => attackpowerPerSeconds;

        public override string ParseDescription()
        {
            return string.Format(Description, attackpowerPerSeconds);
        }

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseAttackPowerBaseOnDurationAlivePerk", menuName = "Definition/Technology/Berserker/IncreaseAttackPowerBaseOnDurationAlivePerk")]
    public class IncreaseAttackPowerBaseOnDurationAlivePerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseAttackPowerBaseOnDurationAlivePerk>
        {
            private float startedAt;
            private Statistic<float> attackPowerFlat;

            public Modifier(IncreaseAttackPowerBaseOnDurationAlivePerk modifierDefinition) : base(modifierDefinition)
            {

            }

            public override void Initialize(ModifierHandler modifiable, ModifierApplier source, List<ModifierParameter> parameters)
            {
                base.Initialize(modifiable, source, parameters);
                startedAt = Time.time;
                attackPowerFlat = new Statistic<float>(StatisticDefinition.FlatAttackPower);
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

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }
    }
}

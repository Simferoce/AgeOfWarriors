using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SpawningMovementSpeedBonusPerkEffect", menuName = "Definition/Technology/Archer/SpawningMovementSpeedBonusPerk/SpawningMovementSpeedBonusPerkEffect")]
    public class SpawningMovementSpeedBonusPerkEffect : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, SpawningMovementSpeedBonusPerkEffect>
        {
            private Statistic<float> speedPercentage;

            public Modifier(SpawningMovementSpeedBonusPerkEffect modifierDefinition) : base(modifierDefinition)
            {

            }

            public override void Initialize(ModifierHandler modifiable, ModifierApplier source, List<ModifierParameter> parameters)
            {
                base.Initialize(modifiable, source, parameters);
                modifiable.Entity.GetCachedComponent<Caster>().OnAbilityUsed += Modifier_OnAbilityUsed;

                speedPercentage = new Statistic<float>(StatisticDefinition.PercentageSpeed, this.GetParameterValue<float>("movementSpeed"));
                StatisticRegistry.Register(speedPercentage);
            }

            private void Modifier_OnAbilityUsed(Ability obj)
            {
                modifiable.RemoveModifier(this);
            }

            public override void Dispose()
            {
                base.Dispose();
                modifiable.Entity.GetCachedComponent<Caster>().OnAbilityUsed -= Modifier_OnAbilityUsed;
                StatisticRegistry.Unregister(speedPercentage);
            }
        }

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }
    }
}

using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SpawningMovementSpeedBonusPerkEffect", menuName = "Definition/Technology/Archer/SpawningMovementSpeedBonusPerk/SpawningMovementSpeedBonusPerkEffect")]
    public class SpawningMovementSpeedBonusPerkEffect : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, SpawningMovementSpeedBonusPerkEffect>
        {
            private Statistic<float> speedPercentage;

            public Modifier(ModifierHandler modifiable, SpawningMovementSpeedBonusPerkEffect modifierDefinition, IModifierSource modifierSource, float movementSpeedIncrease) : base(modifiable, modifierDefinition, modifierSource)
            {
                modifiable.Entity.GetCachedComponent<Caster>().OnAbilityUsed += Modifier_OnAbilityUsed;
                speedPercentage = new Statistic<float>(StatisticDefinition.SpeedPercentage, movementSpeedIncrease);
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
    }
}

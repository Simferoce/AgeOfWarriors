using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SpawningMovementSpeedBonusPerkEffect", menuName = "Definition/Technology/Archer/SpawningMovementSpeedBonusPerk/SpawningMovementSpeedBonusPerkEffect")]
    public class SpawningMovementSpeedBonusPerkEffect : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, SpawningMovementSpeedBonusPerkEffect>
        {
            public override float? SpeedPercentage => movementSpeedIncrease;

            private float movementSpeedIncrease;

            public Modifier(ModifierHandler modifiable, SpawningMovementSpeedBonusPerkEffect modifierDefinition, IModifierSource modifierSource, float movementSpeedIncrease) : base(modifiable, modifierDefinition, modifierSource)
            {
                this.movementSpeedIncrease = movementSpeedIncrease;
                modifiable.Entity.GetCachedComponent<Caster>().OnAbilityUsed += Modifier_OnAbilityUsed;
            }

            private void Modifier_OnAbilityUsed(Ability obj)
            {
                modifiable.RemoveModifier(this);
            }

            public override void Dispose()
            {
                base.Dispose();
                modifiable.Entity.GetCachedComponent<Caster>().OnAbilityUsed -= Modifier_OnAbilityUsed;
            }
        }
    }
}

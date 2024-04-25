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

            public Modifier(IModifiable modifiable, SpawningMovementSpeedBonusPerkEffect modifierDefinition, float movementSpeedIncrease, float duration) : base(modifiable, modifierDefinition)
            {
                With(new CharacterModifierTimeElement(duration));

                this.movementSpeedIncrease = movementSpeedIncrease;
                modifiable.GetCachedComponent<Character>().OnAbilityUsed += Modifier_OnAbilityUsed;
            }

            private void Modifier_OnAbilityUsed(Ability obj)
            {
                modifiable.RemoveModifier(this);
            }

            public override void Dispose()
            {
                base.Dispose();
                modifiable.GetCachedComponent<Character>().OnAbilityUsed -= Modifier_OnAbilityUsed;
            }
        }
    }
}

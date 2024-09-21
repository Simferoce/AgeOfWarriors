using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseDefenseBaseOnAttackPowerPerk", menuName = "Definition/Technology/Shieldbearer/IncreaseDefenseBaseOnAttackPowerPerk")]
    public class IncreaseDefenseBaseOnAttackPowerPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseDefenseBaseOnAttackPowerPerk>
        {
            public override float? AttackPower => Mathf.Floor(modifiable.Entity.GetCachedComponent<Character>().Defense / definition.defenseRequired) * definition.attackPowerIncrease;

            public Modifier(ModifierHandler modifiable, IncreaseDefenseBaseOnAttackPowerPerk modifierDefinition, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
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

using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseDefenseBaseOnAttackPowerPerk", menuName = "Definition/Technology/Shieldbearer/IncreaseDefenseBaseOnAttackPowerPerk")]
    public class IncreaseDefenseBaseOnAttackPowerPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseDefenseBaseOnAttackPowerPerk>
        {
            public override float? Defense => Mathf.Floor(modifiable.GetCachedComponent<Character>().AttackPower / definition.attackPowerRequired) * definition.defenseIncrease;

            public Modifier(IModifiable modifiable, IncreaseDefenseBaseOnAttackPowerPerk modifierDefinition) : base(modifiable, modifierDefinition)
            {
            }
        }

        [SerializeField] private float defenseIncrease;
        [SerializeField] private float attackPowerRequired;

        public override string ParseDescription()
        {
            return $"Increase Defense by {defenseIncrease} for each {attackPowerRequired}";
        }

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this);
        }
    }
}

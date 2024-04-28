using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseDefenseBaseOnAttackPowerPerk", menuName = "Definition/Technology/Shieldbearer/IncreaseDefenseBaseOnAttackPowerPerk")]
    public class IncreaseDefenseBaseOnAttackPowerPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseDefenseBaseOnAttackPowerPerk>
        {
            public override float? AttackPower => Mathf.Floor(modifiable.GetCachedComponent<Character>().Defense / definition.defenseRequired) * definition.attackPowerIncrease;

            public Modifier(IModifiable modifiable, IncreaseDefenseBaseOnAttackPowerPerk modifierDefinition) : base(modifiable, modifierDefinition)
            {
            }
        }

        [SerializeField] private float attackPowerIncrease;
        [SerializeField] private float defenseRequired;

        public override string ParseDescription()
        {
            return string.Format(Description, attackPowerIncrease, defenseRequired);
        }

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this);
        }
    }
}

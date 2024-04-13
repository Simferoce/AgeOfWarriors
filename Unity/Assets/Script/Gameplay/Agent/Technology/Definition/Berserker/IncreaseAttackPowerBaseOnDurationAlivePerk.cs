using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseAttackPowerBaseOnDurationAlivePerk", menuName = "Definition/Technology/Berserker/IncreaseAttackPowerBaseOnDurationAlivePerk")]
    public class IncreaseAttackPowerBaseOnDurationAlivePerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier>
        {
            private float attackPowerPerSecondAlive;
            private float startedAt;

            public override float? AttackPower => attackPowerPerSecondAlive * (Time.time - startedAt);

            public Modifier(IModifiable modifiable, ModifierDefinition modifierDefinition, float attackPowerPerSecondAlive) : base(modifiable, modifierDefinition)
            {
                this.attackPowerPerSecondAlive = attackPowerPerSecondAlive;
                startedAt = Time.time;
            }
        }

        [SerializeField] private float attackPowerPerSecondAlive;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this, attackPowerPerSecondAlive);
        }
    }
}

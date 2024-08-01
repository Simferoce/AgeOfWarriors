using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseAttackPowerBaseOnDurationAlivePerk", menuName = "Definition/Technology/Berserker/IncreaseAttackPowerBaseOnDurationAlivePerk")]
    public class IncreaseAttackPowerBaseOnDurationAlivePerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseAttackPowerBaseOnDurationAlivePerk>
        {
            private float startedAt;

            public override float? AttackPower => definition.attackpowerPerSeconds * (Time.time - startedAt);

            public Modifier(IModifiable modifiable, IncreaseAttackPowerBaseOnDurationAlivePerk modifierDefinition, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
                startedAt = Time.time;
            }
        }

        [SerializeField] private float attackpowerPerSeconds;

        public float AttackPower(Modifier modifier) => attackpowerPerSeconds;

        public override string ParseDescription()
        {
            return string.Format(Description, attackpowerPerSeconds);
        }

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this, modifiable.GetCachedComponent<IModifierSource>());
        }
    }
}

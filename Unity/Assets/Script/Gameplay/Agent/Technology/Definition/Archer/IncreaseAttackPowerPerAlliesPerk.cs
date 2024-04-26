using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseAttackPowerPerAlliesPerk", menuName = "Definition/Technology/Archer/IncreaseAttackPowerPerAlliesPerk")]
    public class IncreaseAttackPowerPerAlliesPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseAttackPowerPerAlliesPerk>
        {
            private int amountOfAlliesPresentOnTheBattleField;

            public override float? AttackPower => amountOfAlliesPresentOnTheBattleField * definition.attackPowerPerAlliesPresent;

            public override bool Show => amountOfAlliesPresentOnTheBattleField > 0;

            public Modifier(IModifiable modifiable, IncreaseAttackPowerPerAlliesPerk modifierDefinition, IModifierSource source = null) : base(modifiable, modifierDefinition, source)
            {
            }

            public override float? GetStack()
            {
                return amountOfAlliesPresentOnTheBattleField;
            }

            public override void Update()
            {
                base.Update();

                amountOfAlliesPresentOnTheBattleField = 0;
                Character character = modifiable.GetCachedComponent<Character>();
                foreach (AgentObject agentObject in AgentObject.All)
                {
                    if (agentObject is not Character)
                        continue;

                    if (agentObject == character)
                        continue;

                    if (agentObject.Faction != character.Faction)
                        continue;

                    amountOfAlliesPresentOnTheBattleField++;
                }
            }
        }

        [SerializeField] private float attackPowerPerAlliesPresent;

        public override string ParseDescription()
        {
            return string.Format(Description, attackPowerPerAlliesPresent);
        }

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this);
        }
    }
}

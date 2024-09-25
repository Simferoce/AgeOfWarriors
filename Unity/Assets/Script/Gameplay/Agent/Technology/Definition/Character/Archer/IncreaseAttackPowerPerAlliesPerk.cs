using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseAttackPowerPerAlliesPerk", menuName = "Definition/Technology/Archer/IncreaseAttackPowerPerAlliesPerk")]
    public class IncreaseAttackPowerPerAlliesPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseAttackPowerPerAlliesPerk>
        {
            private int amountOfAlliesPresentOnTheBattleField;

            //public override float? AttackPower => amountOfAlliesPresentOnTheBattleField * definition.attackPowerPerAlliesPresent;

            public override bool Show => amountOfAlliesPresentOnTheBattleField > 0;

            public Modifier(ModifierHandler modifiable, IncreaseAttackPowerPerAlliesPerk modifierDefinition, IModifierSource source) : base(modifiable, modifierDefinition, source)
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
                Character character = modifiable.Entity.GetCachedComponent<Character>();
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

        [SerializeField] private StatisticSerialize<float> attackPowerPerAlliesPresent = new StatisticSerialize<float>("attack_power_per_allies", null, 1f);

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }
    }
}

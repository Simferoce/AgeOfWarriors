using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseAttackPowerPerAlliesPerk", menuName = "Definition/Technology/Archer/IncreaseAttackPowerPerAlliesPerk")]
    public class IncreaseAttackPowerPerAlliesPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseAttackPowerPerAlliesPerk>
        {
            public override bool Show => amountOfAlliesPresentOnTheBattleField > 0;

            private Statistic<float> attackPowerFlat;
            private int amountOfAlliesPresentOnTheBattleField;

            public Modifier(ModifierHandler modifiable, IncreaseAttackPowerPerAlliesPerk modifierDefinition, IModifierSource source) : base(modifiable, modifierDefinition, source)
            {
                attackPowerFlat = new Statistic<float>(StatisticDefinition.FlatAttackPower);
                StatisticRegistry.Register(attackPowerFlat);
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

                attackPowerFlat.SetValue(amountOfAlliesPresentOnTheBattleField * definition.attackPowerPerAlliesPresent);
            }

            public override void Dispose()
            {
                base.Dispose();
                StatisticRegistry.Unregister(attackPowerFlat);
            }
        }

        [SerializeField] private float attackPowerPerAlliesPresent;

        public override string ParseDescription()
        {
            return string.Format(Description, attackPowerPerAlliesPresent);
        }

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }
    }
}

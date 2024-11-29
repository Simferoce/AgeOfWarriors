using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseAttackSpeedBaseOnNearbyEnemiesPerk", menuName = "Definition/Technology/Shieldbearer/IncreaseAttackSpeedBaseOnNearbyEnemiesPerk")]
    public class IncreaseAttackSpeedBaseOnNearbyEnemiesPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseAttackSpeedBaseOnNearbyEnemiesPerk>
        {
            private int numberOfNearbyEnemies;
            private Statistic<float> attackSpeedPercentage;

            public override bool Show => numberOfNearbyEnemies > 0;

            public Modifier(ModifierHandler modifiable, IncreaseAttackSpeedBaseOnNearbyEnemiesPerk modifierDefinition, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
                attackSpeedPercentage = new Statistic<float>(StatisticDefinition.PercentageAttackSpeed, 0f);
            }

            public override string ParseDescription()
            {
                return string.Format(Definition.Description, definition.attackSpeedIncreasePerEnemies, StatisticFormatter.Percentage(definition.percentageReach, StatisticDefinition.Reach), $"{attackSpeedPercentage.GetValue():0.0%}({numberOfNearbyEnemies}x{definition.attackSpeedIncreasePerEnemies:0.0%})");
            }

            public override void Update()
            {
                base.Update();

                Character character = modifiable.Entity.GetCachedComponent<Character>();

                numberOfNearbyEnemies = 0;

                foreach (AgentObject agent in AgentObject.All)
                {
                    if (agent == character)
                        continue;

                    if (!agent.IsActive)
                        continue;

                    if (!agent.TryGetCachedComponent<Target>(out Target targeteable))
                        continue;

                    if (!agent.TryGetCachedComponent<Attackable>(out Attackable attackable))
                        continue;

                    if (character.Faction == (targeteable.Entity as AgentObject).Faction)
                        continue;

                    if (Mathf.Abs((targeteable.ClosestPoint(character.CenterPosition) - character.CenterPosition).x) > definition.percentageReach * character.Reach)
                        continue;

                    numberOfNearbyEnemies++;
                }

                attackSpeedPercentage.SetValue(numberOfNearbyEnemies * definition.attackSpeedIncreasePerEnemies);
            }

            public override void Dispose()
            {
                base.Dispose();
                StatisticRegistry.Unregister(attackSpeedPercentage);
            }
        }

        [SerializeField, Range(0, 5)] private float percentageReach;
        [SerializeField, Range(0, 5)] private float attackSpeedIncreasePerEnemies;

        public override string ParseDescription()
        {
            return string.Format(Description, attackSpeedIncreasePerEnemies, StatisticFormatter.Percentage(percentageReach, StatisticDefinition.Reach), "");
        }

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }
    }
}

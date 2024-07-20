using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseAttackSpeedBaseOnNearbyEnemiesPerk", menuName = "Definition/Technology/Shieldbearer/IncreaseAttackSpeedBaseOnNearbyEnemiesPerk")]
    public class IncreaseAttackSpeedBaseOnNearbyEnemiesPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseAttackSpeedBaseOnNearbyEnemiesPerk>
        {
            private int numberOfNearbyEnemies;

            public override float? AttackSpeedPercentage => numberOfNearbyEnemies * definition.attackSpeedIncreasePerEnemies;
            public override bool Show => numberOfNearbyEnemies > 0;

            public Modifier(IModifiable modifiable, IncreaseAttackSpeedBaseOnNearbyEnemiesPerk modifierDefinition, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
            }

            public override string ParseDescription()
            {
                return string.Format(Definition.Description, definition.attackSpeedIncreasePerEnemies, StatisticFormatter.Percentage(definition.percentageReach, StatisticDefinition.Reach), $"{AttackSpeedPercentage:0.0%}({numberOfNearbyEnemies}x{definition.attackSpeedIncreasePerEnemies:0.0%})");
            }

            public override void Update()
            {
                base.Update();

                Character character = modifiable.GetCachedComponent<Character>();

                numberOfNearbyEnemies = 0;

                foreach (AgentObject agent in AgentObject.All)
                {
                    if (agent == character)
                        continue;

                    if (!agent.IsActive)
                        continue;

                    if (!agent.TryGetCachedComponent<ITargeteable>(out ITargeteable targeteable))
                        continue;

                    if (!agent.TryGetCachedComponent<IAttackable>(out IAttackable attackable))
                        continue;

                    if (character.Faction == targeteable.Faction)
                        continue;

                    if (Mathf.Abs((targeteable.ClosestPoint(character.CenterPosition) - character.CenterPosition).x) > definition.percentageReach * character.Reach)
                        continue;

                    numberOfNearbyEnemies++;
                }
            }
        }

        [SerializeField, Range(0, 5)] private float percentageReach;
        [SerializeField, Range(0, 5)] private float attackSpeedIncreasePerEnemies;

        public override string ParseDescription()
        {
            return string.Format(Description, attackSpeedIncreasePerEnemies, StatisticFormatter.Percentage(percentageReach, StatisticDefinition.Reach), "");
        }

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this, modifiable.GetCachedComponent<IModifierSource>());
        }
    }
}

using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseAttackSpeedBaseOnNearbyEnemiesPerk", menuName = "Definition/Technology/Shieldbearer/IncreaseAttackSpeedBaseOnNearbyEnemiesPerk")]
    public class IncreaseAttackSpeedBaseOnNearbyEnemiesPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseAttackSpeedBaseOnNearbyEnemiesPerk>
        {
            private int numberOfNearbyEnemies;

            private StatisticModifiable<float> attackSpeedPercentage = new StatisticModifiable<float>(definition: StatisticRepository.AttackSpeedPercentage);

            public override bool Show => numberOfNearbyEnemies > 0;

            public Modifier(ModifierHandler modifiable, IncreaseAttackSpeedBaseOnNearbyEnemiesPerk modifierDefinition, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
                attackSpeedPercentage.Initialize(this);
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

                    if (Mathf.Abs((targeteable.ClosestPoint(character.GetCachedComponent<Target>().CenterPosition) - character.GetCachedComponent<Target>().CenterPosition).x) > definition.percentageReach * character.Reach)
                        continue;

                    numberOfNearbyEnemies++;
                }

                attackSpeedPercentage.Modify(numberOfNearbyEnemies * definition.attackSpeedIncreasePerEnemies);
            }
        }

        [SerializeField, Range(0, 5)] private float percentageReach;
        [SerializeField, Range(0, 5)] private float attackSpeedIncreasePerEnemies;

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }
    }
}

using System;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SpawningMovementSpeedBonusPerk", menuName = "Definition/Technology/Archer/SpawningMovementSpeedBonusPerk/SpawningMovementSpeedBonusPerk")]
    public class SpawningMovementSpeedBonusPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, SpawningMovementSpeedBonusPerk>
        {
            public Modifier(IModifiable modifiable, SpawningMovementSpeedBonusPerk modifierDefinition, IModifierSource source = null) : base(modifiable, modifierDefinition, source)
            {
                Character character = modifiable.GetCachedComponent<Character>();

                foreach (AgentObject agent in AgentObject.All)
                {
                    if (agent == character)
                        continue;

                    if (!agent.IsActive)
                        continue;

                    if (!agent.TryGetCachedComponent<ITargeteable>(out ITargeteable targeteable))
                        continue;

                    if (agent.Faction == character.Faction)
                        continue;

                    if (!agent.TryGetCachedComponent<IAttackable>(out IAttackable attackable))
                        continue;

                    if (Mathf.Abs((targeteable.ClosestPoint(character.CenterPosition) - character.CenterPosition).x) > definition.distance)
                        continue;

                    return;
                }

                modifiable.AddModifier(new SpawningMovementSpeedBonusPerkEffect.Modifier(modifiable, definition.effect, definition.movementSpeedIncrease));
            }
        }

        [SerializeField] private float distance;
        [SerializeField, Range(0, 5)] private float movementSpeedIncrease;
        [SerializeField] private SpawningMovementSpeedBonusPerkEffect effect;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this);
        }
    }
}

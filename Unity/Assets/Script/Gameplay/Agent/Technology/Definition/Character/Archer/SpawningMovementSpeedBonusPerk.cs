using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SpawningMovementSpeedBonusPerk", menuName = "Definition/Technology/Archer/SpawningMovementSpeedBonusPerk/SpawningMovementSpeedBonusPerk")]
    public class SpawningMovementSpeedBonusPerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, SpawningMovementSpeedBonusPerk>
        {
            public Modifier(SpawningMovementSpeedBonusPerk modifierDefinition) : base(modifierDefinition)
            {

            }

            public override void Initialize(ModifierHandler modifiable, ModifierApplier source, List<ModifierParameter> parameters)
            {
                base.Initialize(modifiable, source, parameters);
                Character character = modifiable.Entity.GetCachedComponent<Character>();

                foreach (AgentObject agent in AgentObject.All)
                {
                    if (agent == character)
                        continue;

                    if (!agent.IsActive)
                        continue;

                    if (!agent.TryGetCachedComponent<Target>(out Target targeteable))
                        continue;

                    if (agent.Faction == character.Faction)
                        continue;

                    if (!agent.TryGetCachedComponent<Attackable>(out Attackable attackable))
                        continue;

                    if (Mathf.Abs((targeteable.ClosestPoint(character.CenterPosition) - character.CenterPosition).x) > definition.distance)
                        continue;

                    return;
                }

                ModifierApplier modifierApplier = character.GetCachedComponent<ModifierApplier>();
                modifierApplier.Apply(modifiable, definition.effect.Instantiate(), new List<ModifierParameter>() { new ModifierParameter<float>("movementSpeed", definition.movementSpeedIncrease) });
            }
        }

        [SerializeField] private float distance;
        [SerializeField, Range(0, 5)] private float movementSpeedIncrease;
        [SerializeField] private SpawningMovementSpeedBonusPerkEffect effect;

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }
    }
}

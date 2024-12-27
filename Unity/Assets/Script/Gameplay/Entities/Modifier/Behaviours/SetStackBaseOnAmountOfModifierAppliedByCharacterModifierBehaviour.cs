using Game.Character;
using Game.Statistics;
using System;
using System.Linq;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class SetStackBaseOnAmountOfModifierAppliedByCharacterModifierBehaviour : ModifierBehaviour
    {
        [SerializeField] private ModifierDefinition modifierDefinition;

        private CharacterEntity character;
        private StackModifierBehaviour stackModifierBehaviour;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            character = modifier.GetHierarchy().FirstOrDefault(x => x is CharacterEntity) as CharacterEntity;
            stackModifierBehaviour = modifier.Behaviours.FirstOrDefault(x => x is StackModifierBehaviour) as StackModifierBehaviour;
        }

        public override Result Update()
        {
            int stack = 0;
            CountStack(character, character["faction"].Get<FactionType>(), ref stack);
            stackModifierBehaviour.SetStack(stack);

            return base.Update();
        }

        public void CountStack(Entity entity, FactionType factionType, ref int value)
        {
            if (entity.TryGetCachedComponent<ModifierApplier>(out ModifierApplier modifierApplier))
            {
                foreach (ModifierEntity modifier in modifierApplier.CurrentlyAppliedModifiers.Where(x => modifierDefinition == null || x.Definition == modifierDefinition))
                {
                    if ((!modifier.Target.Entity.StatisticRepository.TryGet("dead", out Statistic<bool> dead) || !dead.Get<bool>())
                        && modifier.Target.Entity["faction"].Get<FactionType>() != factionType)
                    {
                        value += (int)((modifier.Behaviours.FirstOrDefault(x => x is IModifierStack) as IModifierStack)?.CurrentStack ?? 1f);
                    }
                }
            }

            foreach (Entity child in entity.Children)
                CountStack(child, factionType, ref value);
        }
    }
}

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
            CountStack(character, ref stack);
            stackModifierBehaviour.SetStack(stack);

            return base.Update();
        }

        public void CountStack(Entity entity, ref int value)
        {
            if (entity.TryGetCachedComponent<ModifierApplier>(out ModifierApplier modifierApplier))
            {
                foreach (ModifierEntity modifier in modifierApplier.CurrentlyAppliedModifiers.Where(x => x.Definition == modifierDefinition))
                {
                    if ((!modifier.Target.Entity.StatisticRepository.TryGet("dead", out Statistic<bool> dead) || !dead.Get<bool>()))
                    {
                        value += (int)(modifier.Behaviours.FirstOrDefault(x => x is IModifierStack) as IModifierStack).CurrentStack;
                    }
                }
            }

            foreach (Entity child in entity.Children)
                CountStack(child, ref value);
        }
    }
}

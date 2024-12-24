using Game.Character;
using Game.Components;
using System;
using System.Linq;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class SetStackBaseOnTheAmountOfCharacterInBattle : ModifierBehaviour
    {
        [SerializeReference, SubclassSelector] private ModifierTargetFilter filter;

        private StackModifierBehaviour stackModifierBehaviour;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            stackModifierBehaviour = modifier.Behaviours.OfType<StackModifierBehaviour>().FirstOrDefault();
            filter.Initialize(modifier);
        }

        public override Result Update()
        {
            int stack = 0;
            foreach (Target target in Target.All)
            {
                if (target.Entity is not CharacterEntity)
                    continue;

                if (!filter.Execute(target.Entity))
                    continue;

                stack++;
            }

            stackModifierBehaviour.SetStack(stack);
            return base.Update();
        }
    }
}

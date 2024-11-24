using Game.Statistics;
using System;
using System.Linq;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class StackModifierTrigger : ModifierTrigger
    {
        [SerializeField] private StatisticReference<int> threshold;

        private StackModifierBehaviour stackModifierBehaviour;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            threshold.Initialize(modifier);
            stackModifierBehaviour = modifier.Behaviours.FirstOrDefault(x => x is StackModifierBehaviour) as StackModifierBehaviour;
            stackModifierBehaviour.OnStackChanged += StackModifierBehaviour_OnStackChanged;
        }

        public override void Dispose()
        {
            base.Dispose();
            stackModifierBehaviour.OnStackChanged -= StackModifierBehaviour_OnStackChanged;
        }

        private void StackModifierBehaviour_OnStackChanged(int oldValue, int newValue)
        {
            if (newValue >= threshold.Get().GetModifiedValue<int>(Context.Empty))
            {
                Trigger();
            }
        }
    }
}

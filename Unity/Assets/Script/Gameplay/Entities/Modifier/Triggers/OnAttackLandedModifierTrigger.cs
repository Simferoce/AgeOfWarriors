using Game.Components;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class OnAttackLandedModifierTrigger : ModifierTrigger, IModifierTargetProvider
    {
        [SerializeReference, SubclassSelector] private OnAttackLandedModifierTriggerFilter filter;

        private AttackFactory attackFactory;
        private List<object> tagets;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            attackFactory = modifier.Target.Entity.GetCachedComponent<AttackFactory>();
            attackFactory.OnAttackLanded += AttackFactory_OnAttackLanded;
        }

        private void AttackFactory_OnAttackLanded(AttackResult attackResult)
        {
            if (!(filter == null || filter.Execute(attackResult)))
                return;

            tagets = new List<object> { attackResult.Target.Entity };
            Trigger();
        }

        public override void Dispose()
        {
            base.Dispose();
            attackFactory.OnAttackLanded -= AttackFactory_OnAttackLanded;
        }

        public List<object> GetTargets()
        {
            return tagets;
        }
    }
}

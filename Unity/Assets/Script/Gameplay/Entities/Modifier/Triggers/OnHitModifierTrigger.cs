using Game.Character;
using Game.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class OnHitModifierTrigger : ModifierTrigger, IModifierTargetProvider
    {
        [SerializeReference, SubclassSelector] private OnHitModifierTriggerCondition condition;

        private Attackable attackable;
        private Entity source;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            attackable = modifier.Target.Entity.GetCachedComponent<Attackable>();
            attackable.OnDamageTaken += OnDamageTaken;

            if (condition != null)
                condition.Initialize(modifier);
        }

        private void OnDamageTaken(AttackResult result, Attackable receiver)
        {
            if (condition == null || condition.Execute(result, receiver))
            {
                source = result.AttackData.Source.Entity.GetHierarchy().FirstOrDefault(x => x is CharacterEntity);
                Trigger();
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            attackable.OnDamageTaken -= OnDamageTaken;
        }

        public List<object> GetTargets()
        {
            return new List<object> { source };
        }
    }
}

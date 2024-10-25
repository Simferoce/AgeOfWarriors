using Game.Components;
using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class DealDamageModifierEffect : ModifierEffect
    {
        [SerializeReference, SubclassSelector] private ModifierTarget target;
        [SerializeReference, SubclassSelector] private Statistic damage;
        [SerializeField] private AttackData.Flag extraFlags;

        private AttackFactory attackFactory;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            target.Initialize(modifier);
            attackFactory = modifier.AddOrGetCachedComponent<AttackFactory>();
        }

        public override void Execute()
        {
            //foreach (object target in target.GetTargets())
            //{
            //    if (target is Entity entityTarget && entityTarget.TryGetCachedComponent<Attackable>(out Attackable attackable))
            //    {
            //        AttackData attack = attackFactory.Generate(
            //                                target: attackable,
            //                                damage: damage.GetValue<float>(modifier),
            //                                flags: extraFlags
            //                                );

            //        attackable.TakeAttack(attack);
            //    }

            //}
        }
    }
}

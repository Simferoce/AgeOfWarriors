using Game.Components;
using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Ability
{
    [Serializable]
    public class DealDamageAbilityEffect : AbilityEffect
    {
        [SerializeReference, SubclassSelector] private Value leach;
        [SerializeReference, SubclassSelector] private Value damage;
        [SerializeReference, SubclassSelector] private Value armorPenetration;

        public float Damage => damage?.GetValue<float>() ?? 0f;
        public float Leach => leach?.GetValue<float>() ?? 0f;
        public float ArmorPenetration => armorPenetration?.GetValue<float>() ?? 0f;

        public override void Initialize(AbilityEntity ability)
        {
            base.Initialize(ability);
            leach?.Initialize(ability);
            damage?.Initialize(ability);
            armorPenetration?.Initialize(ability);
        }

        public override void Apply()
        {
            if (Ability.Targets.Count == 0)
                return;

            Attackable target = Ability.Targets[0].Entity.GetCachedComponent<Attackable>();

            AttackData attack = Ability.GetCachedComponent<AttackFactory>().Generate(
                target: target,
                damage: Damage,
                armorPenetration: ArmorPenetration,
                leach: Leach);

            target.TakeAttack(attack);
        }
    }
}

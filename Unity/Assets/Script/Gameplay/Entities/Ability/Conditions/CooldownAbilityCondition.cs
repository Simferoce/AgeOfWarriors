using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Ability
{
    [Serializable]
    public class CooldownAbilityCondition : AbilityCondition
    {
        [SerializeField] private StatisticReference<float> cooldown;

        private float lastUsed = 0f;

        public override void Initialize(AbilityEntity ability)
        {
            base.Initialize(ability);
            lastUsed = float.MinValue;
        }

        public override bool Execute()
        {
            return Time.time - lastUsed > cooldown.Resolve(ability).GetValue(null);
        }

        public override void OnAbilityEnded()
        {
            lastUsed = Time.time;
        }
    }
}

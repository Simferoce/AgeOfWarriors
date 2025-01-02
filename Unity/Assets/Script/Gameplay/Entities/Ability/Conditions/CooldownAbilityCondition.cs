using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Ability
{
    [Serializable]
    public class CooldownAbilityCondition : AbilityCondition, ICooldown
    {
        [SerializeField] private StatisticReference<float> cooldown;

        private float lastUsed = 0f;

        public float Remaining => Mathf.Clamp(cooldown.GetOrThrow().Get<float>() - (Time.time - lastUsed), 0, cooldown.GetOrThrow().Get<float>());
        public float Total => cooldown.GetOrThrow().Get<float>();

        public override void Initialize(AbilityEntity ability)
        {
            base.Initialize(ability);
            cooldown.Initialize(ability);
            lastUsed = float.MinValue;
        }

        public override bool Execute()
        {
            return Time.time - lastUsed > cooldown.GetOrThrow().Get<float>();
        }

        public override void OnAbilityEnded()
        {
            lastUsed = Time.time;
        }
    }
}

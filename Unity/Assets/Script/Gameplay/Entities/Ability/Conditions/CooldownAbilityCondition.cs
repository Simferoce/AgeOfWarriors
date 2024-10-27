using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Ability
{
    [Serializable]
    public class CooldownAbilityCondition : AbilityCondition
    {
        [SerializeReference, SubclassSelector] private Statistic cooldown;

        private float lastUsed = 0f;

        public override void Initialize(AbilityEntity ability)
        {
            base.Initialize(ability);
            cooldown.Initialize(ability);
            lastUsed = float.MinValue;
        }

        public override bool Execute()
        {
            return Time.time - lastUsed > cooldown.GetValue<float>(null);
        }

        public override void OnAbilityEnded()
        {
            lastUsed = Time.time;
        }
    }
}

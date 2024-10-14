using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class CooldownCondition : AbilityCondition
    {
        [SerializeField] private Statistic cooldown;

        public float Cooldown => cooldown.GetValue<float>(ability);

        private float lastUsed = 0f;

        public override void Initialize(Ability ability)
        {
            base.Initialize(ability);
            lastUsed = float.MinValue;
        }

        public override bool Execute()
        {
            return Time.time - lastUsed > Cooldown;
        }

        public override void OnAbilityEnded()
        {
            lastUsed = Time.time;
        }
    }
}

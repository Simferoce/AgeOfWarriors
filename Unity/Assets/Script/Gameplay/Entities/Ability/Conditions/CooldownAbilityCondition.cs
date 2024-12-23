﻿using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Ability
{
    [Serializable]
    public class CooldownAbilityCondition : AbilityCondition
    {
        [SerializeField] private StatisticReference<float> cooldown;

        private float lastUsed = 0f;

        public float Cooldown => cooldown.GetOrThrow().Get<float>();

        public override void Initialize(AbilityEntity ability)
        {
            base.Initialize(ability);
            cooldown.Initialize(ability);
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

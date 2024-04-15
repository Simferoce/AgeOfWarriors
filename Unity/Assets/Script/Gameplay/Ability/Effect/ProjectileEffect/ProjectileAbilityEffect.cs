﻿using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ProjectileAbilityEffect : AbilityEffect
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeReference, SubclassSelector] private ProjectileAbilityEffectOrigin origin;

        public override void Apply()
        {
            GameObject gameObject = GameObject.Instantiate(projectilePrefab, origin.GetPosition(this), Quaternion.identity);
            Projectile projectile = gameObject.GetComponent<Projectile>();

            projectile.Initialize(Ability.Character, Ability, Ability.Targets[0] as ITargeteable);
        }
    }
}
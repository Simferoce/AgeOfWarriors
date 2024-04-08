using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ProjectileAbilityEffect : AbilityEffect
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeReference, SerializeReferenceDropdown] private ProjectileAbilityEffectOrigin origin;

        [Space]
        [SerializeReference, SerializeReferenceDropdown] private List<ProjectileData> datas = new List<ProjectileData>();

        public override void Apply()
        {
            GameObject gameObject = GameObject.Instantiate(projectilePrefab, origin.GetPosition(this), Quaternion.identity);
            Projectile projectile = gameObject.GetComponent<Projectile>();

            ProjectileTargetContext targetContext = new ProjectileTargetContext() { Target = Ability.Targets[0] };
            projectile.Initialize(Ability.Character, datas.Select(x => x.GetContext(Ability.Character)).Append(targetContext).ToList());
        }

        public override AbilityEffect Clone()
        {
            ProjectileAbilityEffect projectileAbilityEffect = new ProjectileAbilityEffect();
            projectileAbilityEffect.projectilePrefab = projectilePrefab;
            projectileAbilityEffect.origin = origin.Clone();
            projectileAbilityEffect.datas = datas.Select(x => x.Clone()).ToList();

            return projectileAbilityEffect;
        }
    }
}

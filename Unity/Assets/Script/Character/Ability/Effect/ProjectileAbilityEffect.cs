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
        [SerializeField] private Transform origin;

        [Space]
        [SerializeReference, SubclassSelector] private List<ProjectileData> datas = new List<ProjectileData>();

        public override void Apply()
        {
            GameObject gameObject = GameObject.Instantiate(projectilePrefab, origin.transform.position, Quaternion.identity);
            Projectile projectile = gameObject.GetComponent<Projectile>();

            ProjectileTargetContext targetContext = new ProjectileTargetContext() { Target = ability.Targets[0] };
            projectile.Initialize(ability.Character, datas.Select(x => x.GetContext(ability.Character)).Append(targetContext).ToList());
        }
    }
}

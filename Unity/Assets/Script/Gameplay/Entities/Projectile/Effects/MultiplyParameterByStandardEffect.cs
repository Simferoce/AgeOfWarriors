using Game.Statistics;
using System;
using System.Linq;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class MultiplyParameterByStandardEffect : ProjectileEffect, IProjectileStandardEffect
    {
        [SerializeField] private string parameter;
        [SerializeReference, SubclassSelector] private Value value;

        public override void Initialize(ProjectileEntity projectile)
        {
            base.Initialize(projectile);
            value.Initialize(projectile);
        }

        public void Execute()
        {
            ProjectileParameter<float> projectileParameter = projectile.Parameters.OfType<ProjectileParameter<float>>().FirstOrDefault(x => x.Name == parameter);
            projectileParameter.Modify(projectileParameter.GetValue() * value.GetValue<float>());
        }
    }
}

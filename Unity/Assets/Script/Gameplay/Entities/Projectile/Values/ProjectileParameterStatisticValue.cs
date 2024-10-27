using Game.Extensions;
using Game.Statistics;
using System;
using System.Linq;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class ProjectileParameterStatisticValue<T> : StatisticValue<T>
    {
        [SerializeField] private string name;

        public override T GetValue(Context context)
        {
            if (owner is not ProjectileEntity projectile)
            {
                Debug.LogError($"Expecting the owner to be of type {nameof(ProjectileEntity)} but instead got {context.GetType()}.");
                return default;
            }

            ProjectileParameter<T> projectileParameter = projectile.Parameters.OfType<ProjectileParameter<T>>().FirstOrDefault(x => x.Name == name);
            if (projectileParameter == null)
            {
                Debug.LogError($"Did not find a parameter with name \"{name}\" with type {nameof(T)} in \"{projectile.transform.GetFullPath()}\"", projectile);
                return default;
            }

            return projectileParameter.GetValue();
        }

        public override string GetDescription(Context context)
        {
            return string.Empty;
        }
    }

    [Serializable]
    public class ProjectileParameterStatisticValueFloat : ProjectileParameterStatisticValue<float>
    {

    }
}

using Game.Extensions;
using Game.Statistics;
using System;
using System.Linq;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class ProjectileParameterValue<T> : Value<T>
    {
        [SerializeField] private string name;

        public override T GetValue()
        {
            if (owner is not ProjectileEntity projectile)
            {
                Debug.LogError($"Expecting the owner to be of type {nameof(ProjectileEntity)} but instead got {owner.GetType()}.");
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
    }

    [Serializable]
    public class ProjectileParameterValueFloat : ProjectileParameterValue<float>
    {

    }
}

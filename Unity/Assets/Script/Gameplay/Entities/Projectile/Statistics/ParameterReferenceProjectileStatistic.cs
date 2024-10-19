using System;
using System.Linq;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class ParameterReferenceProjectileStatistic : ProjectileStatistic
    {
        [SerializeField] private string name;

        public override T GetValue<T>(object context)
        {
            if (TryResolve(context, out StatisticProjectileParameter<T> modifierStatistic))
                return modifierStatistic.Value;

            return default(T);
        }

        private bool TryResolve<T>(object context, out T statistic)
            where T : ProjectileParameter
        {
            statistic = null;

            if (context is not ProjectileEntity projectile)
            {
                Debug.LogError($"Expecting the object type of {context} to be of {nameof(ProjectileEntity)}");
                return false;
            }

            ProjectileParameter projectileParameter = projectile.Parameters.FirstOrDefault(x => x.Name == name);
            if (projectileParameter == null)
            {
                Debug.LogError($"Expecting a parameter of name: {name}", projectile);
                return false;
            }

            if (projectileParameter is not T projectileParameterStatistic)
            {
                Debug.LogError($"Expecting the parameter {name} to be of type {nameof(T)}");
                return false;
            }

            statistic = projectileParameterStatistic;
            return true;
        }
    }
}
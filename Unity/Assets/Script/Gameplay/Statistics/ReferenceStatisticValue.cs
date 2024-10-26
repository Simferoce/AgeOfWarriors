using System;
using UnityEngine;

namespace Game.Statistics
{
    [Serializable]
    public class ReferenceStatisticValue<T> : StatisticValue<T>
    {
        [SerializeField] private StatisticReference<T> reference;

        public override T GetValue(Context context)
        {
            return reference.Resolve(owner).GetValue<T>(context);
        }
    }

    [Serializable]
    public class ReferenceStatisticValueFloat : ReferenceStatisticValue<float>
    {

    }
}

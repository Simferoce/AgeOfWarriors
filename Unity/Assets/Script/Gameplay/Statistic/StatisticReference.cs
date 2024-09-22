using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class StatisticReference : Statistic
    {
        [SerializeField] private string path;

        public StatisticReference()
        {

        }

        public StatisticReference(string path)
        {
            this.path = path;
        }

        public override string Name { get => StatisticUtility.Resolve(Context, path)?.Name ?? "Undefined"; set => StatisticUtility.Resolve(Context, path).Name = value; }
        public override StatisticDefinition Definition { get => StatisticUtility.Resolve(Context, path)?.Definition; set => StatisticUtility.Resolve(Context, path).Definition = value; }

        public override bool TryGetValue<T>(out T value)
        {
            value = default;

            if (string.IsNullOrEmpty(path))
                return false;

            return StatisticUtility.Resolve(Context, path)?.TryGetValue(out value) ?? false;
        }
    }
}

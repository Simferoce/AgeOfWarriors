using System;
using UnityEngine;

namespace Game.Components
{
    public static class TargetUtility
    {
        public struct NearestTargetResult
        {
            public Target Target { get; set; }
            public float Distance { get; set; }

            public NearestTargetResult(Target target, float distance)
            {
                Target = target;
                Distance = distance;
            }
        }

        public static bool TryGetNearestTarget(Vector3 position, Func<Target, bool> predicat, out NearestTargetResult nearestTargetResult)
        {
            nearestTargetResult = new NearestTargetResult(null, float.MaxValue);
            foreach (Target target in Target.All)
            {
                if (!predicat(target))
                    continue;

                float distance = Vector3.Distance(position, target.TargetPosition);
                if (distance < nearestTargetResult.Distance)
                {
                    nearestTargetResult.Distance = distance;
                    nearestTargetResult.Target = target;
                }
            }

            return nearestTargetResult.Target != null;
        }
    }
}

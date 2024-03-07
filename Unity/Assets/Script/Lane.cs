using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Lane : MonoBehaviour
    {
        [SerializeField] private Ground ground;
        [SerializeField] private List<SpawnPoint> spawnPoints;

        private List<LaneObject> laneObjects = new List<LaneObject>();

        public Ground Ground { get => ground; }
        public List<SpawnPoint> SpawnPoints { get => spawnPoints; }

        public void Update()
        {
            foreach (LaneObject laneObject in laneObjects)
            {
                laneObject.Tick();
            }
        }

        public void Add(LaneObject laneObject)
        {
            laneObject.Destroyed += OnLaneObjectDestroyed;
            laneObjects.Add(laneObject);
        }

        public void Remove(LaneObject laneObject)
        {
            laneObjects.Remove(laneObject);
        }

        private void OnLaneObjectDestroyed(LaneObject laneObject)
        {
            laneObject.Destroyed -= OnLaneObjectDestroyed;
            Remove(laneObject);
        }

        public float Clamp(float position)
        {
            return ground.Clamp(position);
        }

        public T Cast<T>(float from, float to, out float hit)
            where T : LaneObject
        {
            float direction = Mathf.Sign(to - from);

            foreach (T laneObject in laneObjects.OfType<T>())
            {
                if (direction > 0 && from < laneObject.Min && to > laneObject.Min)
                {
                    hit = laneObject.Min;
                    return laneObject;
                }
                else if (direction < 0 && from > laneObject.Max && to < laneObject.Max)
                {
                    hit = laneObject.Max;
                    return laneObject;
                }
            }

            hit = 0f;
            return null;
        }

        public List<(float, T)> CastAll<T>(float from, float to)
            where T : LaneObject
        {
            List<(float, T)> results = new List<(float, T)>();
            float direction = Mathf.Sign(to - from);

            foreach (T laneObject in laneObjects.OfType<T>())
            {
                if (direction > 0 && from < laneObject.Min && to > laneObject.Min)
                {
                    results.Add((Mathf.Abs(laneObject.Min - from), laneObject));
                }
                else if (direction < 0 && from > laneObject.Max && to < laneObject.Max)
                {
                    results.Add((Mathf.Abs(laneObject.Max - from), laneObject));
                }
            }

            return results;
        }

        public IEnumerable<LaneObject> Intersecting(float min, float max)
        {
            return laneObjects.Where(x => Intersect(x, min, max)).ToList();
        }

        private bool Intersect(LaneObject laneObject, float min, float max)
        {
            return (laneObject.Min > min && laneObject.Min < max)
                || (laneObject.Max > min && laneObject.Max < min)
                || (min > laneObject.Min && min < laneObject.Max)
                || (max > laneObject.Min && max < laneObject.Max);
        }
    }
}
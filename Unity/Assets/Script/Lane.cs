using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Lane : MonoBehaviour
    {
        public static Lane Instance;

        [SerializeField] private float bounds;

        private float TotalLength => bounds * 2;
        private List<LaneObject> laneObjects = new List<LaneObject>();

        private void Awake()
        {
            Instance = this;
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

        public float Clamp(float length)
        {
            return Mathf.Clamp(length, 0, TotalLength);
        }

        public Vector3 GetPosition(float length)
        {
            length = Clamp(length);

            return this.transform.position + (Vector3.right * (length - TotalLength / 2));
        }

        public Vector3 Project(Vector3 point, out float length)
        {
            length = Clamp(bounds + point.x - this.transform.position.x);
            return new Vector3(GetPosition(length).x, this.transform.position.y, point.z);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(this.transform.position + Vector3.right * bounds, 0.1f);
            Gizmos.DrawSphere(this.transform.position - Vector3.right * bounds, 0.1f);
            Gizmos.DrawLine(this.transform.position + Vector3.right * bounds, this.transform.position - Vector3.right * bounds);
        }
    }
}
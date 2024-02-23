using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Lane : MonoBehaviour
{
    [SerializeField] private Spline spline;

    private List<LaneObject> laneObjects = new List<LaneObject>();

    public Spline Spline { get => spline; set => spline = value; }

    public void Initialize()
    {
        foreach(LaneObject laneObject in GameObject.FindObjectsOfType<LaneObject>())
        {
            laneObject.transform.position = spline.Project(laneObject.transform.position, out float position);
            laneObject.Spawn(this, position, 0);
        }
    }

    public void Add(LaneObject laneObject)
    {
        laneObjects.Add(laneObject);
        laneObject.transform.SetParent(this.transform);
    }

    public void Remove(LaneObject laneObject)
    {
        laneObjects.Remove(laneObject);
        laneObject.transform.SetParent(null);
    }

    public void Update()
    {
        foreach(LaneObject laneObject in laneObjects)
        {
            laneObject.Tick();
        }
    }

    public LaneMovementResult CheckMovement(float position, float translation)
    {
        float newPosition = spline.Clamp(position + translation);

        return new LaneMovementResult(newPosition);
    }

    public List<T> Intersecting<T>(float min, float max)
    {
        return laneObjects.Where(x => Intersect(x, min, max)).Select(x => x.GetComponent<T>()).ToList();
    }

    private bool Intersect(LaneObject laneObject, float min, float max)
    {
        return (laneObject.Min > min && laneObject.Min < max)
            || (laneObject.Max > min && laneObject.Max < min)
            || (min > laneObject.Min && min < laneObject.Max)
            || (max > laneObject.Min && max < laneObject.Max);
    }
}


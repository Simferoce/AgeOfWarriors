using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class LaneObject : MonoBehaviour
{
    [SerializeField] private Transform min;
    [SerializeField] private Transform max;
    
    private float position;

    public float Min => position - (this.transform.position - min.position).magnitude;
    public float Max => position + (this.transform.position - max.position).magnitude;
    public float Position
    {
        get
        {
            return position;
        }

        set
        {
            position = value;
            this.transform.position = Lane.Spline.GetPosition(position);
        }
    }

    public Lane Lane { get; protected set; }
    public int Direction { get; protected set; }

    public virtual void Spawn(Lane lane, float position, int direction)
    {
        lane.Add(this);
        if (this.TryGetComponent<Target>(out Target target))
            target.Kill += OnKilled;

        this.Lane = lane;
        this.Position = position;
        this.Direction = direction;
    }

    public virtual void Tick() { }

    public void OnKilled()
    {
        Lane.Remove(this);
        GameObject.Destroy(this.gameObject);
    }
}


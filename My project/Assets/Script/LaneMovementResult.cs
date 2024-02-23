using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public struct LaneMovementResult
{
    public float NewPosition { get; private set; }

    public LaneMovementResult(float newPosition)
    {
        NewPosition = newPosition;
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class LaneObjectDefinition : ScriptableObject
{
    public abstract LaneObject Instantiate(Lane lane, float position, int direction);
}
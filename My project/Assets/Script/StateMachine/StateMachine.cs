using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class StateMachine
{
    private State current;
    private State next;

    public State Current { get => current; }

    public void Initialize(State start)
    {
        current = start;
        current.Enter();
    }

    public void SetState(State state)
    {
        next = state;
    }

    public void Update()
    {
        if(next != null)
        {
            current.Exit();
            current = next;
            current.Enter();

            next = null;
        }

        current.Update();
    }
}

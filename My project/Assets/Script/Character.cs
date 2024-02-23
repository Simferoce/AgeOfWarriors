using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Brain))]
public class Character : LaneObject
{
    private float speed = 0f;
    private CharacterAnimator characterAnimator = null;
    private StateMachine stateMachine = new StateMachine();
    private Brain brain = null;

    public CharacterAnimator CharacterAnimator => characterAnimator;
    public StateMachine StateMachine => stateMachine;
    public Brain Brain => brain;
    public float Speed => speed;

    private void Start()
    {
        characterAnimator = GetComponentInChildren<CharacterAnimator>();
        brain = GetComponent<Brain>();

        brain.Initialize(this);
        stateMachine.Initialize(new CharacterMoveState(this));
    }

    public override void Spawn(Lane lane, float position, int direction)
    {
        this.Spawn(lane, position, direction, 0.0f);
    }

    public void Spawn(Lane lane, float position, int direction, float speed)
    {
        this.speed = speed;
        base.Spawn(lane, position, direction);
    }

    public override void Tick()
    {
        brain.Tick();
        stateMachine.Update();
    }
}


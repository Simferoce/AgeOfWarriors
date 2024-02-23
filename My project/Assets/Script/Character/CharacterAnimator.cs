using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour
{
    public static readonly int SPEED_RATIO = Animator.StringToHash("SpeedRatio");
    public static readonly int ATTACK = Animator.StringToHash("Attack");
    public static readonly int MOVE = Animator.StringToHash("Move");

    private Animator animator;

    public Animator Animator => animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetTrigger(int trigger) 
    {
        animator.SetTrigger(trigger);
    }

    public void SetFloat(int trigger, float value)
    {
        animator.SetFloat(trigger, value);
    }
}


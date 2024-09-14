using Game;
using System;
using UnityEngine;

public class Attackable : MonoBehaviour, IComponent
{
    public Entity Entity { get; set; }
    public event Action<AttackResult, Attackable> OnDamageTaken;

    internal void TakeAttack(Attack attack)
    {
        throw new NotImplementedException();
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;

    public event Action Kill;

    private void Awake()
    {
        this.currentHealth = maxHealth;
    }

    public bool Attackable()
    {
        return currentHealth > 0;
    }

    public void Attack(float damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
            currentHealth = 0;

        if (currentHealth == 0)
            Kill?.Invoke();
    }
}


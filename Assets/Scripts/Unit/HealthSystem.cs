using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;
    public event EventHandler<int> OnDamaged;
    public event EventHandler<int> OnHealed;

    [SerializeField] private int health = 100;
    private int healthMax;

    private void Awake()
    {
        healthMax = health;
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;

        if(health < 0)
        {
            health = 0;
        }

        OnDamaged?.Invoke(this, damageAmount);

        if(health == 0)
        {
            Die();
        }
    }

    public void Heal(int healAmount)
    {
        health += healAmount;

        if (health > healthMax)
        {
            health = healthMax;
        }

        OnHealed?.Invoke(this, healAmount);
    }

    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return (float)health / healthMax;
    }
}

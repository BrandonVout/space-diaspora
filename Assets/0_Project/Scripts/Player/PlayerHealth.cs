using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public EventHandler damaged;
    public EventHandler dead;
    public EventHandler healed;
    public EventHandler recovered;
    [SerializeField] private float recoverTime = 1.0f;
    public EventHandler revived;
    public bool IsHurt { get; private set; }
    public bool IsDead { get; private set; }
    public int Health { get; private set; }

    [field: SerializeField] public int MaxHealth { get; } = 100;

    private void Awake()
    {
        Health = MaxHealth;
    }

    public void Heal(int heal)
    {
        if (IsDead) return;
        IsHurt = false;
        if (Health == MaxHealth) return;

        Health += heal;
        if (Health > MaxHealth)
            Health = MaxHealth;
        healed?.Invoke(this, EventArgs.Empty);
    }

    public void Damage(int damage)
    {
        if (IsDead) return;
        if (IsHurt) return;

        Health -= damage;
        if (Health < 0)
            Health = 0;
        if (Health == 0)
        {
            IsDead = true;
            dead?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            IsHurt = true;
            damaged?.Invoke(this, EventArgs.Empty);
            StartCoroutine(HitRecover());
        }
    }

    public void SetHealth(int health)
    {
        Health = health;
        if (Health > MaxHealth)
            Health = MaxHealth;
        else if (Health < 0)
            Health = 0;
    }

    public void Revive(int health = 0)
    {
        if (!IsDead) return;

        Health = health == 0 ? MaxHealth : health;
        IsDead = false;
        revived?.Invoke(this, EventArgs.Empty);
    }

    public void Kill()
    {
        if (IsDead) return;

        Health = 0;
        IsDead = true;
        dead?.Invoke(this, EventArgs.Empty);
    }

    private IEnumerator HitRecover()
    {
        yield return new WaitForSeconds(recoverTime);
        IsHurt = false;
        recovered?.Invoke(this, EventArgs.Empty);
    }
}
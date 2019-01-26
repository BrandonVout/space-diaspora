using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private float _recoverTime = 1.0f;
    public EventHandler Damaged;
    public EventHandler Dead;
    public EventHandler Healed;
    public EventHandler Recovered;
    public EventHandler Revived;
    public bool IsHurt { get; private set; }
    public bool IsDead { get; private set; }
    public int Health { get; private set; }

    private void Awake()
    {
        Health = _maxHealth;
    }

    public void Heal(int heal)
    {
        if (IsDead) return;
        if (Health == _maxHealth) return;

        Health += heal;
        if (Health > _maxHealth)
            Health = _maxHealth;
        Healed?.Invoke(this, EventArgs.Empty);
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
            Dead?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            IsHurt = true;
            Damaged?.Invoke(this, EventArgs.Empty);
            StartCoroutine(HitRecover());
        }
    }

    public void SetHealth(int health)
    {
        Health = health;
        if (Health > _maxHealth)
            Health = _maxHealth;
        else if (Health < 0)
            Health = 0;
    }

    public void Revive(int health = 0)
    {
        if (!IsDead) return;
        
        Health = health == 0 ? _maxHealth : health;
        IsDead = false;
        Revived?.Invoke(this, EventArgs.Empty);
    }

    public void Kill()
    {
        if (IsDead) return;
        
        Health = 0;
        IsDead = true;
        Dead?.Invoke(this, EventArgs.Empty);
    }

    private IEnumerator HitRecover()
    {
        var timer = 0.0f;

        while (IsHurt)
        {
            if (timer >= _recoverTime) IsHurt = false;
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Recovered?.Invoke(this, EventArgs.Empty);
    }
}
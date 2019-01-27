using System;
using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(FlightMovement2D))]
public class PlayerHurt : MonoBehaviour
{
    private static readonly int IsHurt = Animator.StringToHash("isHurt");
    private static readonly int IsDead = Animator.StringToHash("isDead");
    private FlightMovement2D _movement;
    private PlayerHealth _playerHealth;
    [SerializeField] private Animator animator;
    [SerializeField] private float force = 2.5f;

    // Start is called before the first frame update
    private void Start()
    {
        _movement = GetComponent<FlightMovement2D>();
        _playerHealth = GetComponent<PlayerHealth>();
        _playerHealth.damaged += Damage;
        _playerHealth.recovered += Recover;
        _playerHealth.dead += Dead;
    }

    private void Damage(object sender, EventArgs args)
    {
        _movement.ReflectVelocity(force);
        if (animator != null && _playerHealth.IsHurt) animator.SetBool(IsHurt, true);
    }

    private void Dead(object sender, EventArgs args)
    {
        _movement.ReflectVelocity(force);
        if (animator != null && _playerHealth.IsDead) animator.SetBool(IsDead, true);
    }

    private void Recover(object sender, EventArgs args)
    {
        if (animator != null && !_playerHealth.IsHurt) animator.SetBool(IsHurt, false);
    }
}
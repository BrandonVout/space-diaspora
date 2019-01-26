using System;
using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(FlightMovement2D))]
public class PlayerHurt : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _force = 2.5f;
    private PlayerHealth _playerHealth;
    private FlightMovement2D _movement;

    // Start is called before the first frame update
    private void Start()
    {
        _movement = GetComponent<FlightMovement2D>();
        _playerHealth = GetComponent<PlayerHealth>();
        _playerHealth.Damaged += Damage;
        _playerHealth.Recovered += Recover;
        _playerHealth.Dead += Dead;
    }

    private void Damage(object sender, EventArgs args)
    {
        _movement.ReflectVelocity(_force);
        if (_animator != null && _playerHealth.IsHurt) _animator.SetBool("isHurt", true);
    }

    private void Dead(object sender, EventArgs args)
    {
        _movement.ReflectVelocity(_force);
        if (_animator != null && _playerHealth.IsDead) _animator.SetBool("isDead", true);
    }

    private void Recover(object sender, EventArgs args)
    {
        if (_animator != null && !_playerHealth.IsHurt) _animator.SetBool("isHurt", false);
    }
}
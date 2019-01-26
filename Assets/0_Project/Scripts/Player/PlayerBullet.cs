using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(BulletMovement2D))]
public class PlayerBullet : MonoBehaviour
{
    private BulletMovement2D _movement;
    private CircleCollider2D _collider;

    [SerializeField] private float _speed = 10;
    [SerializeField] private int _damage = 10;
    
    // Start is called before the first frame update
    public void OnShow()
    {
        _movement = GetComponent<BulletMovement2D>();
        _collider = GetComponent<CircleCollider2D>();
    }

    public void Fire(Vector2 dir)
    {
        _movement.ApplyForce(dir * _speed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) return;
    }
}

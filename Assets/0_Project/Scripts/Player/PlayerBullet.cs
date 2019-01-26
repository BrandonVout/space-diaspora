using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(BulletMovement2D))]
public class PlayerBullet : MonoBehaviour
{
    private CircleCollider2D _collider;
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _lifespan = 3.0f;
    private BulletMovement2D _movement;
    [SerializeField] private float _speed = 200;
    public EventHandler Inert;
    public int Damage => _damage;

    public void Initialize()
    {
        _movement = GetComponent<BulletMovement2D>();
        _movement.Initialize();
        _collider = GetComponent<CircleCollider2D>();
    }

    public void Fire(Vector2 dir, Vector3 pos)
    {
        _movement.Stop();
        transform.position = pos;
        _movement.ApplyForce(dir * _speed);
        StartCoroutine(Lifespan());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Projectile") || other.isTrigger) return;

//        var playerHealth = GetComponent<PlayerHealth>();
//        playerHealth.Damage(_damage);
        DestroyBullet();
    }

    private IEnumerator Lifespan()
    {
        yield return new WaitForSeconds(_lifespan);
        DestroyBullet();
    }

    public void DestroyBullet()
    {
        Inert?.Invoke(this, EventArgs.Empty);
        _movement.Stop();
        gameObject.SetActive(false);
    }
}
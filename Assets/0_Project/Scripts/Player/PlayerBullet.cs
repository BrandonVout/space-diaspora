using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(BulletMovement2D))]
public class PlayerBullet : MonoBehaviour
{
    private CircleCollider2D _collider;
    [SerializeField] private float lifespan = 3.0f;
    private BulletMovement2D _movement;
    [SerializeField] private float speed = 200;
    public EventHandler inert;
    [field: SerializeField] public int Damage { get; } = 10;

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
        _movement.ApplyForce(dir * speed);
        StartCoroutine(Lifespan());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Projectile") || other.isTrigger) return;

        DestroyBullet();
    }

    private IEnumerator Lifespan()
    {
        yield return new WaitForSeconds(lifespan);
        DestroyBullet();
    }

    public void DestroyBullet()
    {
        inert?.Invoke(this, EventArgs.Empty);
        _movement.Stop();
        gameObject.SetActive(false);
    }
}
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DestructibleHealth : MonoBehaviour
{
    private Collider2D _collider;
    private Color _color;
    private bool _destroyed;
    private bool _isHurt;
    private SpriteRenderer _sprite;
    [SerializeField] private Color damageColor = Color.red;
    public EventHandler damaged;
    public EventHandler destroyed;
    [SerializeField] private float flashSpeed = 0.1f;
    [SerializeField] private float hurtTime = 0.6f;
    public EventHandler spawned;

    [field: SerializeField] public int MaxHealth { get; } = 30;

    public int Health { get; private set; }

    // Start is called before the first frame update
    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _color = _sprite.color;
        Initialize();
    }

    public void Initialize()
    {
        Health = MaxHealth;
        _isHurt = false;
        _destroyed = false;
        _collider.isTrigger = false;
        var color = _sprite.color;
        color = new Color(color.r, color.g, color.b, 1);
        _sprite.color = color;
        spawned?.Invoke(this, EventArgs.Empty);
    }

    private void Destroy()
    {
        _destroyed = true;
        _collider.isTrigger = true;
        _sprite.color = new Color(_color.r, _color.g, _color.b, 0);
        destroyed?.Invoke(this, EventArgs.Empty);
    }

    private void Damage(int damage)
    {
        if (_isHurt || _destroyed) return;

        Health -= damage;
        if (Health < 0)
            Health = 0;
        if (Health == 0)
            Destroy();
        if (_destroyed) return;
        _isHurt = true;
        StartCoroutine(HurtFlash());
        damaged?.Invoke(this, EventArgs.Empty);
    }

    private IEnumerator HurtFlash()
    {
        var timer = 0.0f;

        while (timer < hurtTime)
        {
            _sprite.color = _sprite.color == _color ? damageColor : _color;
            timer += flashSpeed;
            yield return new WaitForSeconds(flashSpeed);
        }

        _sprite.color = _color;
        _isHurt = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Projectile") || _destroyed || _isHurt) return;

        Damage(other.GetComponent<PlayerBullet>().Damage);
    }
}
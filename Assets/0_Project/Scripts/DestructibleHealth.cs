using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DestructibleHealth : MonoBehaviour
{
    private Collider2D _collider;
    private Color _color;
    [SerializeField] private Color _damageColor = Color.red;
    private bool _destroyed;
    [SerializeField] private float _flashSpeed = 0.1f;
    [SerializeField] private float _hurtTime = 0.6f;
    private bool _isHurt;
    [SerializeField] private int _maxHealth = 30;
    private SpriteRenderer _sprite;
    public EventHandler Damaged;
    public EventHandler Destroyed;
    public EventHandler Spawned;
    public int MaxHealth => _maxHealth;
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
        Health = _maxHealth;
        _isHurt = false;
        _destroyed = false;
        _collider.isTrigger = false;
        _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, 1);
        Spawned?.Invoke(this, EventArgs.Empty);
    }

    private void Destroy()
    {
        _destroyed = true;
        _collider.isTrigger = true;
        _sprite.color = new Color(_color.r, _color.g, _color.b, 0);
        Destroyed?.Invoke(this, EventArgs.Empty);
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
        Damaged?.Invoke(this, EventArgs.Empty);
    }

    private IEnumerator HurtFlash()
    {
        var timer = 0.0f;

        while (timer < _hurtTime)
        {
            _sprite.color = _sprite.color == _color ? _damageColor : _color;
            timer += _flashSpeed;
            yield return new WaitForSeconds(_flashSpeed);
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
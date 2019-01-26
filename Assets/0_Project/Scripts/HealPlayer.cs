using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class HealPlayer : MonoBehaviour
{
    private Color _color;
    [SerializeField] private float _flashSpeed = 0.2f;
    [SerializeField] private int _heal = 10;
    [SerializeField] private Color _healColor = Color.green;
    [SerializeField] private float _healTime = 0.5f;
    [SerializeField] private float _respawnTime = 10.0f;
    private SpriteRenderer _sprite;
    private bool _used;

    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player") || _used) return;
        var player = other.gameObject;

        if (player.GetComponent<PlayerHealth>() == null) return;
        if (player.GetComponent<PlayerHealth>().IsDead) return;

        _used = true;
        _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, 0);
        player.GetComponent<PlayerHealth>().Heal(_heal);
        StartCoroutine(HealFlash(player));
        StartCoroutine(Respawn());
    }

    private IEnumerator HealFlash(GameObject player)
    {
        var spriteRenderer = player.GetComponentInChildren<SpriteRenderer>();
        var timer = 0.0f;

        while (player.GetComponent<PlayerHealth>().IsHurt)
            yield return null;
        _color = spriteRenderer.color;

        while (timer < _healTime)
        {
            spriteRenderer.color = spriteRenderer.color == _color ? _healColor : _color;
            timer += _flashSpeed;
            yield return new WaitForSeconds(_flashSpeed);
        }

        spriteRenderer.color = _color;
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(_respawnTime);
        _used = false;
        _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, 1);
    }
}
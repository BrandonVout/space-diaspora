
using System;
using System.Collections;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    private Color _color;
    [SerializeField] private Color _hurtColor = Color.red;
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _deathFade = 0.5f;
    public EventHandler DamageDealt;
    private bool _inert;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player") || _inert) return;

        DealDamage(other.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player") || _inert) return;

        DealDamage(other.gameObject);
    }

    private void DealDamage(GameObject player)
    {
        if (player.GetComponent<PlayerHealth>() == null) return;
        if (player.GetComponent<PlayerHealth>().IsHurt ||
            player.GetComponent<PlayerHealth>().IsDead) return;

        player.GetComponent<PlayerHealth>().Damage(_damage);
        DamageDealt?.Invoke(this, EventArgs.Empty);
        StartCoroutine(!player.GetComponent<PlayerHealth>().IsDead ? HurtFlash(player) : DeadFlash(player));
    }

    private IEnumerator HurtFlash(GameObject player)
    {
        var spriteRenderer = player.GetComponentInChildren<SpriteRenderer>();
        _color = spriteRenderer.color;

        while (player.GetComponent<PlayerHealth>().IsHurt)
        {
            spriteRenderer.color = spriteRenderer.color == _color ? _hurtColor : _color;
            yield return new WaitForSeconds(0.1f);
        }

        spriteRenderer.color = _color;
    }

    private IEnumerator DeadFlash(GameObject player)
    {
        var spriteRenderer = player.GetComponentInChildren<SpriteRenderer>();
        _color = spriteRenderer.color;
        
        yield return new WaitForSeconds(0.25f);
        spriteRenderer.color = new Color(_color.r, _color.g, _color.b, _deathFade);
        yield return new WaitForSeconds(1.25f);
        spriteRenderer.color = new Color(_color.r, _color.g, _color.b, 0.0f);
    }

    public void SetInert(bool active)
    {
        _inert = active;
    }
}
using System;
using System.Collections;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    private Color _color;
    private bool _inert;
    [SerializeField] private int damage = 10;
    public EventHandler damageDealt;
    [SerializeField] private float deathFade = 0.5f;
    [SerializeField] private Color hurtColor = Color.red;
    [SerializeField] private float hurtFlash = 0.2f;

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

        player.GetComponent<PlayerHealth>().Damage(damage);
        damageDealt?.Invoke(this, EventArgs.Empty);
        StartCoroutine(!player.GetComponent<PlayerHealth>().IsDead ? HurtFlash(player) : DeadFlash(player));
    }

    private IEnumerator HurtFlash(GameObject player)
    {
        var spriteRenderer = player.GetComponentInChildren<SpriteRenderer>();
        _color = spriteRenderer.color;

        while (player.GetComponent<PlayerHealth>().IsHurt)
        {
            spriteRenderer.color = spriteRenderer.color == _color ? hurtColor : _color;
            yield return new WaitForSeconds(hurtFlash);
        }

        spriteRenderer.color = _color;
    }

    private IEnumerator DeadFlash(GameObject player)
    {
        var spriteRenderer = player.GetComponentInChildren<SpriteRenderer>();
        var color = spriteRenderer.color;
        _color = color;

        yield return new WaitForSeconds(0.25f);
        color = new Color(_color.r, _color.g, _color.b, deathFade);
        spriteRenderer.color = color;
        yield return new WaitForSeconds(1.25f);
        color = new Color(_color.r, _color.g, _color.b, 0.0f);
        spriteRenderer.color = color;
    }

    public void SetInert(bool active)
    {
        _inert = active;
    }
}
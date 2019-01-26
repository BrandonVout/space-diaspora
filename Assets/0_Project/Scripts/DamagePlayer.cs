
using System.Collections;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    private Color _color;
    [SerializeField] private Color _hurtColor = Color.red;
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _deathFade = 0.5f;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        var player = other.gameObject;
        
        if (player.GetComponent<PlayerHealth>() == null) return;
        if (player.GetComponent<PlayerHealth>().IsHurt ||
            player.GetComponent<PlayerHealth>().IsDead) return;

        player.GetComponent<PlayerHealth>().Damage(_damage);
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
    }
}
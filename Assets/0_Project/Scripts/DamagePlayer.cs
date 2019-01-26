using System.Collections;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    private Color _color;
    [SerializeField] private Color _hurtColor = Color.red;
    [SerializeField] private int damage = 10;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<PlayerHealth>() == null) return;
        if (other.gameObject.GetComponent<PlayerHealth>().IsHurt ||
            other.gameObject.GetComponent<PlayerHealth>().IsDead) return;

        other.gameObject.GetComponent<PlayerHealth>().Damage(damage);
        StartCoroutine(HurtFlash(other.gameObject));
    }

    private IEnumerator HurtFlash(GameObject player)
    {
        var renderer = player.GetComponentInChildren<SpriteRenderer>();
        _color = renderer.color;

        while (player.GetComponent<PlayerHealth>().IsHurt && !player.GetComponent<PlayerHealth>().IsDead)
        {
            renderer.color = renderer.color == _color ? _hurtColor : _color;
            yield return new WaitForSeconds(0.1f);
        }

        renderer.color = _color;
    }
}
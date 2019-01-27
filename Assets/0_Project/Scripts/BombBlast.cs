using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(DamagePlayer))]
[RequireComponent(typeof(SpriteRenderer))]
public class BombBlast : MonoBehaviour
{
    private DamagePlayer _damage;
    private SpriteRenderer _sprite;
    [SerializeField] private bool respawn;
    [SerializeField] private float respawnTime = 10.0f;

    // Start is called before the first frame update
    private void Start()
    {
        _damage = GetComponent<DamagePlayer>();
        _sprite = GetComponent<SpriteRenderer>();

        _damage.damageDealt += Explode;
    }

    private void Explode(object sender, EventArgs args)
    {
        _damage.SetInert(true);
        var color = _sprite.color;
        color = new Color(color.r, color.g, color.b, 0);
        _sprite.color = color;
        if (respawn)
            StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        var color = _sprite.color;
        color = new Color(color.r, color.g, color.b, 1);
        _sprite.color = color;
        _damage.SetInert(false);
    }
}
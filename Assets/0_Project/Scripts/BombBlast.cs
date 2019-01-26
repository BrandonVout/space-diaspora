using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DamagePlayer))]
[RequireComponent(typeof(SpriteRenderer))]
public class BombBlast : MonoBehaviour
{
    [SerializeField] private float _respawnTime = 10.0f;
    private DamagePlayer _damage;
    private SpriteRenderer _sprite;
    
    // Start is called before the first frame update
    void Start()
    {
        _damage = GetComponent<DamagePlayer>();
        _sprite = GetComponent<SpriteRenderer>();

        _damage.DamageDealt += Explode;
    }

    private void Explode(object sender, EventArgs args)
    {
        _damage.SetInert(true);
        _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, 0);
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(_respawnTime);
        _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, 1);
        _damage.SetInert(false);
    }
}

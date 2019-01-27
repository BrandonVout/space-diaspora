using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
public class HealPlayer : MonoBehaviour
{
    private AudioSource _source;
    private Color _color;
    private SpriteRenderer _sprite;
    private bool _used;
    [SerializeField] private float flashSpeed = 0.2f;
    [SerializeField] private int heal = 10;
    [SerializeField] private Color healColor = Color.green;
    [SerializeField] private float healTime = 0.5f;
    [SerializeField] private bool respawn;
    [SerializeField] private float respawnTime = 10.0f;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player") || _used) return;
        var player = other.gameObject;

        if (player.GetComponent<PlayerHealth>() == null) return;
        if (player.GetComponent<PlayerHealth>().IsDead) return;

        _used = true;
        _source.Play();
        _sprite.color = new Color(_color.r, _color.g, _color.b, 0);
        player.GetComponent<PlayerHealth>().Heal(heal);
        StartCoroutine(HealFlash(player));
        if (respawn)
            StartCoroutine(Respawn());
    }

    private IEnumerator HealFlash(GameObject player)
    {
        var spriteRenderer = player.GetComponentInChildren<SpriteRenderer>();
        var timer = 0.0f;

        while (player.GetComponent<PlayerHealth>().IsHurt)
            yield return null;
        _color = spriteRenderer.color;

        while (timer < healTime)
        {
            spriteRenderer.color = spriteRenderer.color == _color ? healColor : _color;
            timer += flashSpeed;
            yield return new WaitForSeconds(flashSpeed);
        }

        spriteRenderer.color = _color;
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        _used = false;
        var color = _sprite.color;
        color = new Color(color.r, color.g, color.b, 1);
        _sprite.color = color;
    }
}
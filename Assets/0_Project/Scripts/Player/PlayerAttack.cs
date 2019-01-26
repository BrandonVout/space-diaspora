using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float _coolDown = 0.25f;
    [SerializeField] private GameObject _bulletPrefab;
    private PlayerHealth _health;
    public EventHandler Attack;
    private bool _waiting;
    private GameObject[] _bullets = new GameObject[16];

    // Start is called before the first frame update
    private void Start()
    {
        _health = GetComponent<PlayerHealth>();
        for (int i = 0; i < _bullets.Length; i++)
        {
            _bullets[i] = Instantiate(_bulletPrefab, new Vector2(0, 0), Quaternion.identity);
            _bullets[i].transform.parent = null;
            _bullets[i].SetActive(false);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (_health.IsDead) return;
        if (_waiting) return;

        if(Input.GetButton("Fire1"))
            Fire();
    }

    private void Fire()
    {
        if (_bulletPrefab == null) return;
        _waiting = true;
        
        _bulletPrefab.GetComponent<PlayerBullet>().Fire(new Vector2(1, 0));
        Attack?.Invoke(this, EventArgs.Empty);
    }

    private IEnumerator CoolDown()
    {
        var timer = 0.0f;

        while (_waiting)
        {
            _waiting = timer < _coolDown;
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
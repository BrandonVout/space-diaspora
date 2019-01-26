using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(PlayerHealth))]
public class PlayerAttack : MonoBehaviour
{
    private int _activeBullet;
    public FireType ActiveFire { get; private set; }
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private GameObject[] _bullets = new GameObject[32];
    [SerializeField] private float _coolDown = 1.5f;
    private float _changeDelay = 0.5f;
    private PlayerHealth _health;
    private readonly int _scatterCount = 4;
    private bool _waiting;
    private bool _changing;
    public EventHandler Attack;
    public EventHandler CooledDown;
    public EventHandler ChangedWeapon;

    // Start is called before the first frame update
    private void Start()
    {
        ActiveFire = FireType.SingleFire;
        _health = GetComponent<PlayerHealth>();
        _activeBullet = 0;
        for (var i = 0; i < _bullets.Length; i++)
        {
            _bullets[i] = Instantiate(_bulletPrefab, new Vector2(0, 0), Quaternion.identity);
            _bullets[i].transform.parent = null;
            _bullets[i].GetComponent<PlayerBullet>().Initialize();
            _bullets[i].SetActive(false);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (_health.IsDead) return;
        if (_waiting) return;

        if (Input.GetButton("Fire1") && !_waiting)
            Fire();

        if (!(Mathf.Abs(Input.mouseScrollDelta.y) > 0) || _changing) return;
        
        _changing = true;
        if (Input.mouseScrollDelta.y > 0)
        {
            if (ActiveFire > 0)
                ActiveFire--;
            else
                ActiveFire = FireType.RandomFire;
        }
        else
        {
            if (ActiveFire < FireType.RandomFire)
                ActiveFire++;
            else
                ActiveFire = FireType.SingleFire;
        }
        StartCoroutine(WeaponDelay());
        ChangedWeapon?.Invoke(this, EventArgs.Empty);
    }

    private void Fire()
    {
        _waiting = true;
        switch (ActiveFire)
        {
            case FireType.SingleFire:
                SingleFire();
                break;
            case FireType.CrossFire:
                CrossFire();
                break;
            case FireType.XFire:
                XFire();
                break;
            case FireType.CircleFire:
                CircleFire();
                break;
            case FireType.RandomFire:
                RandomFire();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        StartCoroutine(CoolDown());
        Attack?.Invoke(this, EventArgs.Empty);
    }

    private void SingleFire()
    {
        var velocity = GetComponent<FlightMovement2D>().GetVelocity();

        _bullets[_activeBullet].SetActive(true);
        _bullets[_activeBullet].GetComponent<PlayerBullet>().Fire(velocity.normalized, transform.position);
        _activeBullet++;
        if (_activeBullet >= _bullets.Length)
            _activeBullet = 0;
    }

    private void CrossFire()
    {
        for (var i = 0; i < _scatterCount; i++)
        {
            Vector2 pos = transform.position;
            var angle = Mathf.PI * 2 * i / _scatterCount;
            pos.y = Mathf.Sin(angle);
            pos.x = Mathf.Cos(angle);

            _bullets[_activeBullet].SetActive(true);
            _bullets[_activeBullet].GetComponent<PlayerBullet>().Fire(pos.normalized, transform.position);
            _activeBullet++;
            if (_activeBullet >= _bullets.Length)
                _activeBullet = 0;
        }
    }

    private void XFire()
    {
        for (var i = 1; i < _scatterCount * 2; i += 2)
        {
            Vector2 pos = transform.position;
            var angle = Mathf.PI * 2 * i  / (_scatterCount * 2);
            pos.y = Mathf.Sin(angle);
            pos.x = Mathf.Cos(angle);

            _bullets[_activeBullet].SetActive(true);
            _bullets[_activeBullet].GetComponent<PlayerBullet>().Fire(pos.normalized, transform.position);
            _activeBullet++;
            if (_activeBullet >= _bullets.Length)
                _activeBullet = 0;
        }
    }

    private void CircleFire()
    {
        for (var i = 0; i < _scatterCount * 3; i++)
        {
            Vector2 pos = transform.position;
            var angle = Mathf.PI * 2 * i / (_scatterCount * 3);
            pos.y = Mathf.Sin(angle);
            pos.x = Mathf.Cos(angle);

            _bullets[_activeBullet].SetActive(true);
            _bullets[_activeBullet].GetComponent<PlayerBullet>().Fire(pos.normalized, transform.position);
            _activeBullet++;
            if (_activeBullet >= _bullets.Length)
                _activeBullet = 0;
        }
    }

    private void RandomFire()
    {
        var r = UnityEngine.Random.Range(0.1f, 5.0f);
        for (var i = 0; i < _scatterCount; i++)
        {
            Vector2 pos = transform.position;
            var angle = Mathf.PI * 2 * ((float)i / _scatterCount) * r;
            pos.y = Mathf.Sin(angle);
            pos.x = Mathf.Cos(angle);

            _bullets[_activeBullet].SetActive(true);
            _bullets[_activeBullet].GetComponent<PlayerBullet>().Fire(pos.normalized, transform.position);
            _activeBullet++;
            if (_activeBullet >= _bullets.Length)
                _activeBullet = 0;
        }
    }

    private IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(_coolDown);
        _waiting = false;
        CooledDown?.Invoke(this, EventArgs.Empty);
    }

    private IEnumerator WeaponDelay()
    {
        yield return new WaitForSeconds(_changeDelay);
        _changing = false;
        CooledDown?.Invoke(this, EventArgs.Empty);
    }

    public enum FireType
    {
        SingleFire,
        CrossFire,
        XFire,
        CircleFire,
        RandomFire
    }
}
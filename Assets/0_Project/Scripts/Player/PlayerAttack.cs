using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PlayerHealth))]
public class PlayerAttack : MonoBehaviour
{
    public enum FireType
    {
        SingleFire,
        CrossFire,
        XFire,
        CircleFire,
        RandomFire
    }

    private const float ChangeDelay = 0.2f;
    private const int ScatterCount = 4;
    private int _activeBullet;
    private bool _changing;
    private float _coolDown;
    private PlayerHealth _health;
    private bool _waiting;
    [SerializeField] private Animator animator;
    public EventHandler attack;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject[] bullets = new GameObject[32];
    public EventHandler changedWeapon;
    public EventHandler cooledDown;
    private static readonly int Attacking = Animator.StringToHash("attacking");
    public FireType ActiveFire { get; private set; }

    // Start is called before the first frame update
    private void Start()
    {
        ActiveFire = FireType.SingleFire;
        _health = GetComponent<PlayerHealth>();
        _activeBullet = 0;
        for (var i = 0; i < bullets.Length; i++)
        {
            bullets[i] = Instantiate(bulletPrefab, new Vector2(0, 0), Quaternion.identity);
            bullets[i].transform.parent = null;
            bullets[i].GetComponent<PlayerBullet>().Initialize();
            bullets[i].SetActive(false);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (_health.IsDead) return;
        if (_waiting) return;

        if (Input.GetButton("Fire1") && !_waiting && !_health.IsHurt)
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
        changedWeapon?.Invoke(this, EventArgs.Empty);
    }

    private void Fire()
    {
        _waiting = true;
        if (animator != null) animator.SetBool(Attacking, true);
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
        attack?.Invoke(this, EventArgs.Empty);
    }

    private void SingleFire()
    {
        _coolDown = 0.5f;
        var velocity = GetComponent<FlightMovement2D>().GetVelocity();

        bullets[_activeBullet].SetActive(true);
        bullets[_activeBullet].GetComponent<PlayerBullet>().Fire(
            velocity.normalized == Vector2.zero ? new Vector2(0, -1) : velocity.normalized, transform.position);
        _activeBullet++;
        if (_activeBullet >= bullets.Length)
            _activeBullet = 0;
    }

    private void CrossFire()
    {
        _coolDown = 1.0f;
        for (var i = 0; i < ScatterCount; i++)
        {
            Vector2 pos = transform.position;
            var angle = Mathf.PI * 2 * i / ScatterCount;
            pos.y = Mathf.Sin(angle);
            pos.x = Mathf.Cos(angle);

            bullets[_activeBullet].SetActive(true);
            bullets[_activeBullet].GetComponent<PlayerBullet>().Fire(pos.normalized, pos);
            _activeBullet++;
            if (_activeBullet >= bullets.Length)
                _activeBullet = 0;
        }
    }

    private void XFire()
    {
        _coolDown = 1.0f;
        for (var i = 1; i < ScatterCount * 2; i += 2)
        {
            Vector2 pos = transform.position;
            var angle = Mathf.PI * 2 * i / (ScatterCount * 2);
            pos.y = Mathf.Sin(angle);
            pos.x = Mathf.Cos(angle);

            bullets[_activeBullet].SetActive(true);
            bullets[_activeBullet].GetComponent<PlayerBullet>().Fire(pos.normalized, pos);
            _activeBullet++;
            if (_activeBullet >= bullets.Length)
                _activeBullet = 0;
        }
    }

    private void CircleFire()
    {
        _coolDown = 1.5f;
        for (var i = 0; i < ScatterCount * 3; i++)
        {
            Vector2 pos = transform.position;
            var angle = Mathf.PI * 2 * i / (ScatterCount * 3);
            pos.y = Mathf.Sin(angle);
            pos.x = Mathf.Cos(angle);

            bullets[_activeBullet].SetActive(true);
            bullets[_activeBullet].GetComponent<PlayerBullet>().Fire(pos.normalized, pos);
            _activeBullet++;
            if (_activeBullet >= bullets.Length)
                _activeBullet = 0;
        }
    }

    private void RandomFire()
    {
        _coolDown = 0.75f;
        var r = Random.Range(0.1f, 5.0f);
        for (var i = 0; i < ScatterCount; i++)
        {
            Vector2 pos = transform.position;
            var angle = Mathf.PI * 2 * ((float) i / ScatterCount) * r;
            pos.y = Mathf.Sin(angle);
            pos.x = Mathf.Cos(angle);

            bullets[_activeBullet].SetActive(true);
            bullets[_activeBullet].GetComponent<PlayerBullet>().Fire(pos.normalized, pos);
            _activeBullet++;
            if (_activeBullet >= bullets.Length)
                _activeBullet = 0;
        }
    }

    private IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(_coolDown);
        _waiting = false;
        if (animator != null) animator.SetBool(Attacking, false);
        cooledDown?.Invoke(this, EventArgs.Empty);
    }

    private IEnumerator WeaponDelay()
    {
        yield return new WaitForSeconds(ChangeDelay);
        _changing = false;
        cooledDown?.Invoke(this, EventArgs.Empty);
    }
}
/*---------------------------------------
Brandon Vout

- FlightController2D
    - Translates inputs to movement script
---------------------------------------*/

using UnityEngine;

[RequireComponent(typeof(FlightMovement2D))]
[RequireComponent(typeof(PlayerHealth))]
public class FlightController2D : MonoBehaviour
{
    private PlayerHealth _health;
    private FlightMovement2D _movement;

    [SerializeField] private float _speed = 10.0f;

    // Start is called before the first frame update
    private void Start()
    {
        _movement = GetComponent<FlightMovement2D>();
        _health = GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (_health.IsDead) return;

        var vertical = Input.GetAxis("Vertical") * _speed;
        var horizontal = Input.GetAxis("Horizontal") * _speed;

        _movement.ApplyForce(new Vector2(horizontal, vertical));
    }
}
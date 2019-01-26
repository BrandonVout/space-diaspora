/*---------------------------------------
Brandon Vout

- FlightMovement2D
    - To be used by player and enemy controllers to move rigidbodies
---------------------------------------*/

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FlightMovement2D : MonoBehaviour
{
    private Vector2 _lastPosition;
    private Rigidbody2D _rigidbody;
    private Vector2 _velocity;
    [SerializeField] private float _maxVelocity = 10.0f;

    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _velocity = Vector2.zero;
        _lastPosition = transform.position;
    }

    private void Update()
    {
        _velocity = (Vector2) transform.position - _lastPosition;
        _lastPosition = transform.position;
    }

    public void ApplyForce(Vector2 force)
    {
        _rigidbody.AddForce(force);
        _rigidbody.velocity = Vector2.ClampMagnitude(_rigidbody.velocity, _maxVelocity);
    }

    public Vector2 GetVelocity()
    {
        return _velocity;
    }

    public void ReflectVelocity(float force)
    {
        var velocity = _velocity;

        _rigidbody.velocity = Vector2.zero;
        _velocity = Vector2.zero;
        _lastPosition = transform.position;
        _rigidbody.AddForce(new Vector2(-velocity.normalized.x * force, -velocity.normalized.y * force),
            ForceMode2D.Impulse);
    }
}
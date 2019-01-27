/*---------------------------------------
Brandon Vout

- FlightMovement2D
    - To be used by player and enemy controllers to move rigidbodies
---------------------------------------*/

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletMovement2D : MonoBehaviour
{
    private Vector2 _lastPosition;
    private Rigidbody2D _rigidbody;
    private Vector2 _velocity;

    // Start is called before the first frame update
    public void Initialize()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _velocity = Vector2.zero;
        _lastPosition = transform.position;
    }

    private void Update()
    {
        var position = transform.position;
        _velocity = (Vector2) position - _lastPosition;
        _lastPosition = position;
    }

    public void ApplyForce(Vector2 force)
    {
        _rigidbody.AddForce(force);
    }

    public Vector2 GetVelocity()
    {
        return _velocity;
    }

    public void Stop()
    {
        _rigidbody.velocity = Vector2.zero;
        _velocity = Vector2.zero;
        _lastPosition = transform.position;
    }
}
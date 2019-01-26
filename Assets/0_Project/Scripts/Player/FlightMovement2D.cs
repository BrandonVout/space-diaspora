/*---------------------------------------
Brandon Vout

- FlightMovement2D
    - To be used by player and enemy controllers to move rigidbodies
---------------------------------------*/

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FlightMovement2D : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;

    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void ApplyForce(Vector2 force)
    {
        _rigidbody2D.AddForce(force);
    }

    public Vector2 GetVelocity()
    {
        return _rigidbody2D.velocity;
    }

    public void ReflectVelocity(float force)
    {
        _rigidbody2D.velocity = new Vector2(-_rigidbody2D.velocity.x + force, -_rigidbody2D.velocity.y + force);
    }
}
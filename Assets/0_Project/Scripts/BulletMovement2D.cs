/*---------------------------------------
Brandon Vout

- FlightMovement2D
    - To be used by player and enemy controllers to move rigidbodies
---------------------------------------*/

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletMovement2D : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void ApplyForce(Vector2 force)
    {
        _rigidbody.AddForce(force);
    }

    public Vector2 GetVelocity()
    {
        return _rigidbody.velocity;
    }
}
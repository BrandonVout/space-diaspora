using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    private GameObject _oldTarget;
    private float _zPos;
    private float _yOff;
    [SerializeField] private GameObject target;

    // Start is called before the first frame update
    private void Start()
    {
        _zPos = transform.position.z;
        _yOff = target.transform.position.y;
        _oldTarget = target;
    }

    // Update is called once per frame
    private void Update()
    {
        var position = target.transform.position;
        transform.position = new Vector3(position.x, position.y - _yOff, _zPos);
    }

    public void ChangeTarget(GameObject newTarget)
    {
        _oldTarget = target;
        target = newTarget;
    }

    public void RevertTarget()
    {
        var tempTarget = target;
        target = _oldTarget;
        _oldTarget = tempTarget;
    }
}
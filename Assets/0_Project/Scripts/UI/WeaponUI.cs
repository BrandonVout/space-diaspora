using UnityEngine;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private PlayerAttack _player;
    [SerializeField] private Color _selectColor = Color.magenta;

    // Start is called before the first frame update
    private void Start()
    {
        if (_player == null) return;
    }

    // Update is called once per frame
    private void Update()
    {
    }
}
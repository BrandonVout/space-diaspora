using System;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private PlayerAttack _player;
    [SerializeField] private Color _selectColor = Color.magenta;
    [SerializeField] private GameObject[] _weaponUI = new GameObject[(int)PlayerAttack.FireType.RandomFire];
    private Color _color;
    private PlayerAttack.FireType _activeWeapon;

    // Start is called before the first frame update
    private void Start()
    {
        if (_player == null) return;

        _activeWeapon = _player.ActiveFire;
        _color = _weaponUI[(int)_activeWeapon].GetComponent<Image>().color;
        _weaponUI[(int)_activeWeapon].GetComponent<Image>().color = _selectColor;

        _player.ChangedWeapon += Select;
    }
    
    private void Select(object sender, EventArgs args)
    {
        _weaponUI[(int)_activeWeapon].GetComponent<Image>().color = _color;
        _activeWeapon = _player.ActiveFire;
        _color = _weaponUI[(int)_activeWeapon].GetComponent<Image>().color;
        _weaponUI[(int)_activeWeapon].GetComponent<Image>().color = _selectColor;
    }
}
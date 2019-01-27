using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class WeaponUI : MonoBehaviour
{
    private AudioSource _source;
    [SerializeField] private PlayerAttack player;
    [SerializeField] private Color selectColor = Color.magenta;
    [SerializeField] private GameObject[] weaponUi = new GameObject[(int)PlayerAttack.FireType.RandomFire + 1];
    private Color _color;
    private PlayerAttack.FireType _activeWeapon;

    // Start is called before the first frame update
    private void Start()
    {
        if (player == null) return;

        _source = GetComponent<AudioSource>();
        _activeWeapon = player.ActiveFire;
        _color = weaponUi[(int)_activeWeapon].GetComponent<Image>().color;
        weaponUi[(int)_activeWeapon].GetComponent<Image>().color = selectColor;

        player.changedWeapon += Select;
    }
    
    private void Select(object sender, EventArgs args)
    {
        _source.Play();
        weaponUi[(int)_activeWeapon].GetComponent<Image>().color = _color;
        _activeWeapon = player.ActiveFire;
        _color = weaponUi[(int)_activeWeapon].GetComponent<Image>().color;
        weaponUi[(int)_activeWeapon].GetComponent<Image>().color = selectColor;
    }
}
using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PlayerHealthUI : MonoBehaviour
{
    private float _healthBarLength;
    private Image _image;
    [SerializeField] private PlayerHealth _player;

    // Start is called before the first frame update
    private void Start()
    {
        if (_player == null) return;

        _image = GetComponent<Image>();
        _healthBarLength = (float) _player.Health / _player.MaxHealth;
        _player.Damaged += UpdateHealthBar;
        _player.Healed += UpdateHealthBar;
        _player.Dead += UpdateHealthBar;
        _player.Revived += UpdateHealthBar;
    }

    private void UpdateHealthBar(object sender, EventArgs args)
    {
        _healthBarLength = (float) _player.Health / _player.MaxHealth;
        if (_healthBarLength > 0.8f && _healthBarLength < 1f)
            _image.fillAmount = 0.85f;
        else if (_healthBarLength > 0.0f && _healthBarLength < 0.2f)
            _image.fillAmount = 0.15f;
        else
            _image.fillAmount = _healthBarLength;
    }
}
using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PlayerHealthUI : MonoBehaviour
{
    private float _healthBarLength;
    private Image _image;
    [SerializeField] private PlayerHealth player;

    // Start is called before the first frame update
    private void Start()
    {
        if (player == null) return;

        _image = GetComponent<Image>();
        _healthBarLength = (float) player.Health / player.MaxHealth;
        player.damaged += UpdateHealthBar;
        player.healed += UpdateHealthBar;
        player.dead += UpdateHealthBar;
        player.revived += UpdateHealthBar;
    }

    private void UpdateHealthBar(object sender, EventArgs args)
    {
        _healthBarLength = (float) player.Health / player.MaxHealth;
        if (_healthBarLength > 0.8f && _healthBarLength < 1f)
            _image.fillAmount = 0.85f;
        else if (_healthBarLength > 0.0f && _healthBarLength < 0.2f)
            _image.fillAmount = 0.15f;
        else
            _image.fillAmount = _healthBarLength;
    }
}
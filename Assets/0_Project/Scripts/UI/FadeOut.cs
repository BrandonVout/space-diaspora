using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FadeOut : MonoBehaviour
{
    private SpriteRenderer _sprite;
    [SerializeField] private float fadeSeconds = 1.0f;
    [SerializeField] private float maxAlpha = 1.0f;
    [SerializeField] private float minAlpha;

    // Start is called before the first frame update
    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    public void FadeSpriteOut()
    {
        StopCoroutine(FadeTextToMinAlpha());
    }

    public void FadeSpriteIn()
    {
        StopCoroutine(FadeTextToMaxAlpha());
    }

    private IEnumerator FadeTextToMaxAlpha()
    {
        var color = _sprite.color;
        color = new Color(color.r, color.g, color.b, minAlpha);
        _sprite.color = color;
        while (_sprite.color.a < maxAlpha)
        {
            color = new Color(color.r, color.g, color.b, color.a + Time.deltaTime / fadeSeconds);
            _sprite.color = color;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator FadeTextToMinAlpha()
    {
        var color = _sprite.color;
        color = new Color(color.r, color.g, color.b, maxAlpha);
        _sprite.color = color;
        while (_sprite.color.a > minAlpha)
        {
            color = new Color(color.r, color.g, color.b, color.a - Time.deltaTime / fadeSeconds);
            _sprite.color = color;
            yield return new WaitForEndOfFrame();
        }
    }
}
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeText : MonoBehaviour
{
    [SerializeField] private bool autoPlay;
    [SerializeField] private Text[] counterText;
    [SerializeField] private float fadeTime = 1.0f;
    [SerializeField] private float frameTime = 3.0f; // How long frames are shown
    [SerializeField] private float pauseTime = 0.25f; // Time spent before slideshow starts
    public EventHandler showsOver;
    private bool _showsOn;

    [Tooltip("How opaque the text will become.")] [SerializeField]
    private float maxAlpha = 1.0f; // How opaque the text will become

    [Tooltip("How transparent the text will become.")] [SerializeField]
    private float minAlpha; // How transparent the text will become

    // Start is called before the first frame update
    private void Start()
    {
        if (autoPlay)
            SlideShow();
    }

    public void FadeIn(Text message)
    {
        StartCoroutine(FadeTextToMaxAlpha(message));
    }

    public void FadeOut(Text message)
    {
        StartCoroutine(FadeTextToMinAlpha(message));
    }

    public void SlideShow()
    {
        if (_showsOn) return;
        _showsOn = true;
        StartCoroutine(Transition());
    }

    private IEnumerator Transition()
    {
        yield return new WaitForSeconds(pauseTime);
        foreach (var message in counterText)
        {
            StartCoroutine(FadeTextToMaxAlpha(message));
            yield return new WaitForSeconds(fadeTime);
            yield return new WaitForSeconds(frameTime);
            StartCoroutine(FadeTextToMinAlpha(message));
            yield return new WaitForSeconds(fadeTime);
            yield return new WaitForSeconds(pauseTime);
        }

        _showsOn = false;
        showsOver?.Invoke(this, EventArgs.Empty);
    }

    private IEnumerator FadeTextToMaxAlpha(Graphic message)
    {
        var color = message.color;
        color = new Color(color.r, color.g, color.b, minAlpha);
        message.color = color;
        while (message.color.a < maxAlpha)
        {
            color = new Color(color.r, color.g, color.b, color.a + Time.deltaTime / fadeTime);
            message.color = color;
            yield return null;
        }
    }

    private IEnumerator FadeTextToMinAlpha(Graphic message)
    {
        var color = message.color;
        color = new Color(color.r, color.g, color.b, maxAlpha);
        message.color = color;
        while (message.color.a > minAlpha)
        {
            color = new Color(color.r, color.g, color.b, color.a - Time.deltaTime / fadeTime);
            message.color = color;
            yield return null;
        }
    }
}
/*
-----------------------------------------------------------------------------
        Created By Brandon Vout
-----------------------------------------------------------------------------
*/

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Timer
{
    [RequireComponent(typeof(IntTimerWarning))]
    public class IntTimerWarningText : MonoBehaviour
    {
        private Color _defaultColor;
        private IntTimerWarning _intTimerWarning;

        [Tooltip("How opaque the text will become in warning flash.")] [SerializeField] [Range(0.0f, 1.0f)]
        private float _maxAlpha = 1.0f; // How opaque the text will become

        [Tooltip("How transparent the text will become in warning flash.")] [SerializeField] [Range(0.0f, 1.0f)]
        private float _minAlpha; // How transparent the text will become

        [SerializeField] private Text _timeText;

        [SerializeField] private Color _warningColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);

        private bool _warningFlash;

        [Tooltip("The interval of the warning flash.")] [SerializeField]
        private float _warningInterval = 1.0f;

        /// <summary> Use this for initialization </summary>
        private void Awake()
        {
            _warningFlash = false;
        }

        /// <summary> Use this for initialization </summary>
        private void Start()
        {
            _intTimerWarning = gameObject.GetComponent<IntTimerWarning>();

            _intTimerWarning.TimerWarningOn += Timer_Warning_On;
            _intTimerWarning.TimerWarningOff += Timer_Warning_Off;

            if (_timeText != null)
                _defaultColor = _timeText.color;
        }

        private IEnumerator WarningFlash()
        {
            while (_warningFlash)
            {
                StartCoroutine(FadeTextToMaxAlpha(_warningInterval * 0.3f));
                yield return new WaitForSeconds(_warningInterval * 0.7f);
                StartCoroutine(FadeTextToMinAlpha(_warningInterval * 0.3f));
                yield return new WaitForSeconds(_warningInterval * 0.3f);
            }

            _timeText.color = _defaultColor;
            yield return null;
        }

        private IEnumerator FadeTextToMaxAlpha(float seconds)
        {
            while (_timeText.color.a < _maxAlpha && _warningFlash)
            {
                _timeText.color = new Color(_timeText.color.r, _timeText.color.g, _timeText.color.b,
                    _timeText.color.a + Time.deltaTime / seconds);
                yield return new WaitForEndOfFrame();
            }

            if (!_warningFlash)
                _timeText.color = _defaultColor;
            yield return null;
        }

        private IEnumerator FadeTextToMinAlpha(float seconds)
        {
            while (_timeText.color.a > _minAlpha && _warningFlash)
            {
                _timeText.color = new Color(_timeText.color.r, _timeText.color.g, _timeText.color.b,
                    _timeText.color.a - Time.deltaTime / seconds);
                yield return new WaitForEndOfFrame();
            }

            if (!_warningFlash)
                _timeText.color = _defaultColor;
            yield return null;
        }

        private void Timer_Warning_On(object sender, EventArgs args)
        {
            if (_warningFlash)
                return;

            _timeText.color = new Color(_warningColor.r, _warningColor.g, _warningColor.b, _timeText.color.a);
            _warningFlash = true;
            StartCoroutine(WarningFlash());
        }

        private void Timer_Warning_Off(object sender, EventArgs args)
        {
            _warningFlash = false;
        }
    }
}
/*
-----------------------------------------------------------------------------
        Created By Brandon Vout
-----------------------------------------------------------------------------
*/

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Timer
{
    [RequireComponent(typeof(FloatTimer))]
    public class FloatTimerText : MonoBehaviour
    {
        private FloatTimer _floatTimer;
        [SerializeField] [Range(1, 4)] private int _milliSecDecimalPoints = 2;

        private string _milliSecFormat;

        [SerializeField] private Text _timeText;

        [SerializeField] private bool _useMinutesSecondsFormat;

        /// <summary> Update _timeText.text to match current Timer Time. </summary>
        private void UpdateCanvas()
        {
            if (_timeText == null)
                return;

            if (_useMinutesSecondsFormat)
                _timeText.text = (int) (_floatTimer.Time / 60) + ":" +
                                (_floatTimer.Time % 60).ToString(_milliSecFormat);
            else
                _timeText.text = _floatTimer.Time.ToString("f" + _milliSecDecimalPoints);
        }

        /// <summary> Event called when _floatTimer.TimerUpdated is Invoked. Does nothing but call UpdateCanvas(). </summary>
        private void Timer_Updated(object sender, EventArgs args)
        {
            UpdateCanvas();
        }

        /// <summary> Use this for initialization </summary>
        private void Awake()
        {
            if (_useMinutesSecondsFormat)
            {
                _milliSecFormat = "00.";
                for (var i = 0; i < _milliSecDecimalPoints; i++)
                    _milliSecFormat += "0";
            }
        }

        /// <summary> Use this for initialization </summary>
        private void Start()
        {
            _floatTimer = gameObject.GetComponent<FloatTimer>();
            _floatTimer.TimerUpdated += Timer_Updated;
            UpdateCanvas();
        }
    }
}
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
    [RequireComponent(typeof(IntTimer))]
    public class IntTimerText : MonoBehaviour
    {
        private IntTimer _intTimer;

        [SerializeField] private Text _timeText;

        [SerializeField] private bool _useMinutesSecondsFormat;

        /// <summary> Use this for initialization </summary>
        private void Start()
        {
            _intTimer = gameObject.GetComponent<IntTimer>();
            _intTimer.TimerUpdated += Timer_Updated;
            UpdateCanvas();
        }

        private void UpdateCanvas()
        {
            if (_timeText == null)
                return;

            if (_useMinutesSecondsFormat)
            {
                _timeText.text = _intTimer.Time / 60 + ":";
                if (_intTimer.Time % 60 < 10)
                    _timeText.text += "0";
                _timeText.text += (_intTimer.Time % 60).ToString();
            }
            else
            {
                _timeText.text = _intTimer.Time.ToString();
            }
        }

        private void Timer_Updated(object sender, EventArgs args)
        {
            UpdateCanvas();
        }
    }
}
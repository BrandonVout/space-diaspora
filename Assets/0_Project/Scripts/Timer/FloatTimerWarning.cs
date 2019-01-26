/*
-----------------------------------------------------------------------------
        Created By Brandon Vout
-----------------------------------------------------------------------------
 */

using System;
using UnityEngine;

namespace Timer
{
    [RequireComponent(typeof(FloatTimer))]
    public class FloatTimerWarning : MonoBehaviour
    {
        private FloatTimer _floatTimer;

        /// <summary> When timer passes this number, warning is on. </summary>
        [SerializeField] private float _warnTime;

        public EventHandler TimerWarningOff;

        public EventHandler TimerWarningOn;

        public bool IsWarning { get; private set; }

        private void WarningCheck()
        {
            switch (_floatTimer.Type)
            {
                case TimerType.CountDown:
                    if (!IsWarning && _floatTimer.Time <= _warnTime)
                        Timer_Warning_On();
                    else if (IsWarning && _floatTimer.Time > _warnTime)
                        Timer_Warning_Off();
                    break;
                case TimerType.CountUp:
                    if (!IsWarning && _floatTimer.Time >= _warnTime)
                        Timer_Warning_On();
                    else if (IsWarning && _floatTimer.Time < _warnTime)
                        Timer_Warning_Off();
                    break;
                case TimerType.None:
                    break;
                default:
                    break;
            }
        }

        /// <summary> Use this for initialization </summary>
        private void Awake()
        {
            IsWarning = false;
        }

        /// <summary> Use this for initialization </summary>
        private void Start()
        {
            _floatTimer = gameObject.GetComponent<FloatTimer>();
            _floatTimer.TimerUpdated += Timer_Updated;
        }

        /// <summary> Invoke event when timer passes warning. </summary>
        private void Timer_Warning_On()
        {
            IsWarning = true;
            TimerWarningOn?.Invoke(this, EventArgs.Empty);
        }

        /// <summary> Invoke event when timer is no longer below warning time. </summary>
        private void Timer_Warning_Off()
        {
            IsWarning = false;
            TimerWarningOff?.Invoke(this, EventArgs.Empty);
        }

        private void Timer_Updated(object sender, EventArgs args)
        {
            WarningCheck();
        }
    }
}
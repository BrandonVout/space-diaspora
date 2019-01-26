/*
-----------------------------------------------------------------------------
        Created By Brandon Vout
-----------------------------------------------------------------------------
 */

using System;
using UnityEngine;

namespace Timer
{
    [RequireComponent(typeof(IntTimer))]
    public class IntTimerWarning : MonoBehaviour
    {
        private IntTimer _intTimer;

        /// <summary> When timer passes this number, warning is on. </summary>
        [SerializeField] private int _warnTime;

        public EventHandler TimerWarningOff;

        public EventHandler TimerWarningOn;

        public bool IsWarning { get; private set; }

        private void WarningCheck()
        {
            switch (_intTimer.Type)
            {
                case TimerType.CountDown:
                    if (!IsWarning && _intTimer.Time <= _warnTime)
                        Timer_Warning_On();
                    else if (IsWarning && _intTimer.Time > _warnTime)
                        Timer_Warning_Off();
                    break;
                case TimerType.CountUp:
                    if (!IsWarning && _intTimer.Time >= _warnTime)
                        Timer_Warning_On();
                    else if (IsWarning && _intTimer.Time < _warnTime)
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
            _intTimer = gameObject.GetComponent<IntTimer>();

            _intTimer.TimerUpdated += Timer_Updated;
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
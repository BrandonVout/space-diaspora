/*
-----------------------------------------------------------------------------
        Created By Brandon Vout
-----------------------------------------------------------------------------
*/

using System;
using System.Collections;
using UnityEngine;

namespace Timer
{
    public class FloatTimer : MonoBehaviour
    {
        // Not constants to allow serialization
        [SerializeField] private float _maxTime = 90.0f;
        [SerializeField] private float _minTime;

        [SerializeField] private bool _playOnStart; // Start timer at Start()

        [SerializeField] private float _startTime;

        [SerializeField] private TimerType _timerType = TimerType.None;
        public EventHandler TimerPaused;
        public EventHandler TimerReset;
        public EventHandler TimerStarted;
        public EventHandler TimerUp;

        public EventHandler TimerUpdated;
        public TimerType Type => _timerType;

        public float StartTime => _startTime;

        public float Time { get; private set; }

        public bool IsTimerOn { get; private set; }

        public bool IsTimeUp { get; private set; }

        public float MaxTimer => _maxTime;
        public float MinTimer => _minTime;

        /// <summary> Use this for initialization </summary>
        private void Awake()
        {
            IsTimeUp = false;
            SetTime(_startTime);
        }

        /// <summary> Use this for initialization </summary>
        private void Start()
        {
            Timer_Update();
            if (_playOnStart)
                StartTimer();
        }

        public void CountUp()
        {
            _timerType = TimerType.CountUp;
        }

        public void CountDown()
        {
            _timerType = TimerType.CountDown;
        }

        public void StartTimer()
        {
            IsTimerOn = true;
            Timer_Start();
            StartCoroutine(RunTimer());
        }

        public void PauseTimer()
        {
            IsTimerOn = false;
            Timer_Pause();
        }

        /// <summary> Reset Time to _startTime and IsTimeUp to false. Invoke Reset and Update Events. </summary>
        public void ResetTimer()
        {
            Time = _startTime;
            IsTimeUp = false;
            Timer_Reset();
            Timer_Update();
        }

        public void AddTime(float deltaTime)
        {
            Time = Mathf.Min(Time + deltaTime, _maxTime);
            Timer_Update();
        }

        public void SubtractTime(float deltaTime)
        {
            Time = Mathf.Max(Time - deltaTime, _minTime);
            Timer_Update();
        }

        public void SetTime(float newTime)
        {
            if (newTime >= _minTime && newTime <= _maxTime)
                Time = newTime;
            else if (newTime < _minTime)
                Time = _minTime;
            else // if (newTime > maxTime)
                Time = _maxTime;
            Timer_Update();
        }

        private IEnumerator RunTimer()
        {
            yield return null;
            do
            {
                Time = UpdateTime();
                Timer_Update();
                yield return new WaitForEndOfFrame();
            } while (IsTimerOn);
        }

        private float UpdateTime()
        {
            switch (_timerType)
            {
                case TimerType.CountDown:
                    return Mathf.Max(Time - UnityEngine.Time.deltaTime, _minTime);
                case TimerType.CountUp:
                    return Mathf.Min(Time + UnityEngine.Time.deltaTime, _maxTime);
                case TimerType.None:
                    return 0.0f;
                default: // Do Nothing
                    return 0.0f;
            }
        }

        private void TimeUpCheck()
        {
            if (IsTimeUp)
                return;

            switch (_timerType)
            {
                case TimerType.CountDown:
                    if (Time <= _minTime)
                        Timer_Finished();
                    break;
                case TimerType.CountUp:
                    if (Time >= _maxTime)
                        Timer_Finished();
                    break;
                case TimerType.None:
                    break;
            }
        }

        /// <summary> Invoke event when timer is updated. </summary>
        private void Timer_Update()
        {
            TimeUpCheck();
            TimerUpdated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary> Invoke event when timer is up. </summary>
        private void Timer_Finished()
        {
            IsTimeUp = true;
            IsTimerOn = false;
            TimerUp?.Invoke(this, EventArgs.Empty);
        }

        /// <summary> Invoke event when timer passes warning. </summary>
        private void Timer_Start()
        {
            TimerStarted?.Invoke(this, EventArgs.Empty);
        }

        /// <summary> Invoke event when timer passes warning. </summary>
        private void Timer_Pause()
        {
            TimerPaused?.Invoke(this, EventArgs.Empty);
        }

        /// <summary> Invoke event when timer passes warning. </summary>
        private void Timer_Reset()
        {
            TimerReset?.Invoke(this, EventArgs.Empty);
        }
    }
}
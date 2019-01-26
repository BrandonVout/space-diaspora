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
    public class IntTimer : MonoBehaviour
    {
        // Not constants to allow serialization
        [SerializeField] private int _maxTime = 90;
        [SerializeField] private int _minTime;

        [SerializeField] private bool _playOnStart; // Start timer at Start()

        [SerializeField] private int _startTime = 5;

        [Tooltip("1.0 == 1 second interval during count down, e.g. change to 2.0 to count down every 2 seconds")]
        [SerializeField]
        private float _timerInterval = 1.0f; // E.G. 1 == 1 second intervals

        [SerializeField] private TimerType _timerType = TimerType.None;

        public EventHandler TimerPaused;
        public EventHandler TimerReset;
        public EventHandler TimerStarted;
        public EventHandler TimerUp;
        public EventHandler TimerUpdated;

        public TimerType Type => _timerType;

        public int StartTime => _startTime;

        public int Time { get; private set; }

        public bool IsTimerOn { get; private set; }

        public bool IsTimeUp { get; private set; }

        public int MaxTimer => _maxTime;
        public int MinTimer => _minTime;

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

        public void ResetTimer()
        {
            Time = _startTime;
            IsTimeUp = false;
            Timer_Reset();
            Timer_Update();
        }

        public void AddTime(int deltaTime)
        {
            Time = Mathf.Min(Time + deltaTime, _maxTime);
            Timer_Update();
        }

        public void SubtractTime(int deltaTime)
        {
            Time = Mathf.Max(Time - deltaTime, _minTime);
            Timer_Update();
        }

        public void SetTime(int newTime)
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
                yield return new WaitForSeconds(_timerInterval);
                Time = UpdateTime();
                Timer_Update();
            } while (IsTimerOn);
        }

        private int UpdateTime()
        {
            switch (_timerType)
            {
                case TimerType.CountDown:
                    return Mathf.Max(Time - 1, _minTime);
                case TimerType.CountUp:
                    return Mathf.Min(Time + 1, _maxTime);
                case TimerType.None:
                    return 0;
                default:    // Do Nothing
                    return 0;
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
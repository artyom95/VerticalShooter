using System;
using Cysharp.Threading.Tasks;
using Settings;

namespace UI.StartScreen
{
    public class StartScreenModel
    {
        public event Action<float> SetRemainingTime;
        public event Action StopCountDown;
        private float _countdownTime;
        private int _delay;

        public void Initialize(GameSettings gameSettings)
        {
            _countdownTime = gameSettings.TimerSettings.CountdownTime;
            _delay = gameSettings.TimerSettings.Delay;
        }

        public async void BeginCountDown()
        {
            var timeRemaining = _countdownTime;

            while (timeRemaining >= 0)
            {
                SetRemainingTime?.Invoke(timeRemaining);
                await UniTask.Delay(_delay); 
                timeRemaining -= 1; 
            }

            StopCountDown?.Invoke();
        }
    }
}
using System;
using Settings;
using UnityEngine;

namespace Enemy
{
    public class EnemySpawnTimer : MonoBehaviour

    {
        public event Action TimerFinishedEvent;
        public event Action CheckAliveEnemies;

        private float _spawnTimerValue;
        private float _currentTime;
        private bool _shouldStartTimer;
        private bool _shouldStartCheckAliveEnemies;

        public void Initialize(GameSettings gameSettings)
        {
            _shouldStartTimer = true;
            _spawnTimerValue = gameSettings.SpawnTimer;
            _currentTime = gameSettings.SpawnTimer;
        }

        /// <summary>
        /// should think about checking alive enemies
        /// if to be more precise in with what period of time i have to start do it   
        /// </summary>
        public void Update()
        {
            if (!_shouldStartTimer)
            {
                return;
            }

            _currentTime += Time.deltaTime;
            CheckCurrentTimer();

            if (_shouldStartCheckAliveEnemies)
            {
                CheckAliveEnemies?.Invoke();
            }
        }

        public void StopTimer()
        {
            _shouldStartTimer = false;
        }

        private void CheckCurrentTimer()
        {
            if (_currentTime > _spawnTimerValue)
            {
                _currentTime = 0;
                TimerFinishedEvent?.Invoke();
                _shouldStartCheckAliveEnemies = true;
            }
        }
    }
}
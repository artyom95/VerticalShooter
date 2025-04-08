using System;
using Settings;
using UnityEngine;

namespace Enemy
{
    public class EnemySpawnTimer : MonoBehaviour

    {
        public event Action TimerFinishedEvent;
        public event Action CheckAliveEnemiesEvent;

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
                CheckAliveEnemiesEvent?.Invoke();
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
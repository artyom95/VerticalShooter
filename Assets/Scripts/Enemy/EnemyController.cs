using System;
using System.Collections.Generic;
using Settings;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemyController : IDisposable

    {
        public event Action<int> AmountEnemyChangedAction;
        public event Action AmountEnemyEndedAction;
        public event Action KillAllEnemies; 
        public event Action<Enemy> EnemySpawned;
        public event Action<Enemy> EnemyDied;
        public event Action<Enemy,float> HealthChangedAction;
        
        private readonly GameSettings _gameSettings;
        private readonly Enemy _enemyPrefab;
        private readonly EnemyFactory _enemyFactory;
        private readonly List<Transform> _spawnPoints;
        private readonly EnemySpawnTimer _enemySpawnTimer;
        private readonly EnemyCounter _enemyCounter;
        private Action<Enemy> _enemyDestroyed;
        
        private Enemy _enemy;

        public EnemyController(GameSettings gameSettings, Enemy enemyPrefab,
            List<Transform> spawnPoints, EnemySpawnTimer enemySpawnTimer)
        {
            _enemySpawnTimer = enemySpawnTimer;
            _spawnPoints = spawnPoints;
            _enemyPrefab = enemyPrefab;
            _gameSettings = gameSettings;
            _enemyFactory = new EnemyFactory();
            _enemyCounter = new EnemyCounter(_gameSettings,
                OnAmountEnemyChanged, OnAmountEnemyEnded,
                OnAliveEnemyEnded);
        }

        public void Initialize(Action<Enemy> enemyDestroyedAction)
        {
            _enemySpawnTimer.Initialize(_gameSettings);
            _enemyDestroyed = enemyDestroyedAction;
            Subscribe();
        }

        public void KillAliveEnemies()
        {
            KillAllEnemies?.Invoke();
            _enemyCounter.KillAllEnemies();
            UnSubscribe();
        }

        public void UnSubscribe()
        {
            _enemySpawnTimer.TimerFinishedEvent -= CreateEnemy;
            _enemySpawnTimer.CheckAliveEnemies -= CheckAliveEnemies;
            _enemySpawnTimer.StopTimer();
        }

        public void Dispose()
        {
            UnSubscribe();
        }

        private void Subscribe()
        {
            _enemySpawnTimer.TimerFinishedEvent += CreateEnemy;
            _enemySpawnTimer.CheckAliveEnemies += CheckAliveEnemies;
        }

        private void CreateEnemy()
        {
            var spawnPoint = GetSpawnPoint();
            _enemy = _enemyFactory.Create(_enemyPrefab, spawnPoint);
            
            _enemy.Initialize(_gameSettings, _enemyDestroyed, 
                _enemyCounter.DeleteEnemy, EnemyDied, OnHealthChanged);
            _enemyCounter.AddEnemy(_enemy);
            _enemy.StartMove();
            EnemySpawned?.Invoke(_enemy);
            _enemyCounter.DecreaseAmountEnemy();
        }

        private void CheckAliveEnemies()
        {
            _enemyCounter.CheckAmountAliveEnemies();
        }

        private Transform GetSpawnPoint()
        {
            var indexTransform = Random.Range(0, 2);
            return _spawnPoints[indexTransform];
        }

        private void OnAmountEnemyChanged(int amountEnemy)
        {
            AmountEnemyChangedAction?.Invoke(amountEnemy);
        }

        private void OnAmountEnemyEnded()
        {
            _enemySpawnTimer.TimerFinishedEvent -= CreateEnemy;
        }

        private void OnAliveEnemyEnded()
        {
            AmountEnemyEndedAction?.Invoke();
        }

        private void OnHealthChanged(Enemy enemy, float health)
        {
            HealthChangedAction?.Invoke(enemy, health);
        }
    }
}
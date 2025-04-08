using System;
using System.Collections.Generic;
using Settings;
using UnityEngine;
using UnityEngine.Pool;
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
        public event Action<Enemy, float> HealthChangedAction;

        private readonly GameSettings _gameSettings;
        private readonly Enemy _enemyPrefab;
        private readonly EnemyFactory _enemyFactory;
        private readonly List<Transform> _spawnPoints;
        private readonly EnemySpawnTimer _enemySpawnTimer;
        private readonly EnemyCounter _enemyCounter;
        private Action<Enemy> _enemyDestroyed;
        private ObjectPool<Enemy> _enemyPool;

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
            _enemyPool = new ObjectPool<Enemy>(OnEnemyCreate, OnTake,
                OnRealise, OnDestroyAction,
                true, 5,
                10);
            Subscribe();
        }

        public void KillAliveEnemies()
        {
            KillAllEnemies?.Invoke();
            _enemyCounter.KillAllEnemies();
            _enemyPool.Clear();
            UnSubscribe();
        }

        public void UnSubscribe()
        {
            _enemySpawnTimer.TimerFinishedEvent -= CreateEnemy;
            _enemySpawnTimer.CheckAliveEnemiesEvent -= CheckAliveEnemiesEvent;
            _enemySpawnTimer.StopTimer();
        }

        public void Dispose()
        {
            UnSubscribe();
        }

        private void Subscribe()
        {
            _enemySpawnTimer.TimerFinishedEvent += CreateEnemy;
            _enemySpawnTimer.CheckAliveEnemiesEvent += CheckAliveEnemiesEvent;
        }

        private void CreateEnemy()
        {
            _enemy = _enemyPool.Get();
            _enemyCounter.AddEnemy(_enemy);
            _enemy.StartMove();
            EnemySpawned?.Invoke(_enemy);
            _enemyCounter.DecreaseAmountEnemy();
        }

        private void CheckAliveEnemiesEvent()
        {
            _enemyCounter.CheckAmountAliveEnemies();
        }

        private Transform GetSpawnPoint()
        {
            var indexTransform = Random.Range(0, 3);
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

        private void OnTake(Enemy enemy)
        {
            enemy.gameObject.SetActive(true);
        }

        private void OnDestroyAction(Enemy enemy)
        {
            enemy.ReleaseEnemyAction -= ReleaseEnemy;
            enemy.Destroy();
        }

        private void OnRealise(Enemy enemy)
        {
            enemy.SetDefaultProperties();
            var spawnPoint = GetSpawnPoint();
            enemy.gameObject.transform.position = spawnPoint.position;
            enemy.gameObject.SetActive(false);
        }

        private Enemy OnEnemyCreate()
        {
            var spawnPoint = GetSpawnPoint();
            var enemy = _enemyFactory.Create(_enemyPrefab, spawnPoint);
            enemy.ReleaseEnemyAction += ReleaseEnemy;
            enemy.Initialize(_gameSettings, _enemyDestroyed,
                _enemyCounter.DeleteEnemy, EnemyDied, OnHealthChanged);
            return enemy;
        }

        private void ReleaseEnemy(Enemy enemy)
        {
            _enemyPool.Release(enemy);
        }
    }
}
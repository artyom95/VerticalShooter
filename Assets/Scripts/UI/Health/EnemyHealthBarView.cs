using System.Collections.Generic;
using Settings;
using UnityEngine;
using UnityEngine.Pool;

namespace UI.Health
{
    public class EnemyHealthBarView
    {
        private readonly Dictionary<Enemy.Enemy, HealthBar> _enemyHealthBarDictionary = new();
        private Dictionary<Enemy.Enemy, HealthBar> _copyEnemyHealthBarDictionary = new();
        private ObjectPool<HealthBar> _healthBarObjectPool;
        private Transform _healthBarParent;
        private HealthBar _healthBar;
        private float _maxHealth;
        private HealthBar _healthBarPrefab;
        private HealthBarFactory _healthBarFactory;

        public void Initialize(GameSettings gameSettings,
            HealthBar healthBarPrefab,
            HealthBarFactory healthBarFactory,
            Transform healthBarParent)
        {
            _healthBarFactory = healthBarFactory;
            _healthBarPrefab = healthBarPrefab;
            _maxHealth = gameSettings.EnemyHealth;
            _healthBarParent = healthBarParent;
            _healthBarObjectPool = new ObjectPool<HealthBar>(OnHealthBarCreate, OnTake,
                OnRealise, OnDestroyAction,
                true, 4,
                10);
        }

        public void CreateHealthBar(Enemy.Enemy enemy)
        {
            _healthBar = _healthBarObjectPool.Get();
            _enemyHealthBarDictionary.Add(enemy, _healthBar);
        }

        public void ShowHealth(Enemy.Enemy enemy, float health)
        {
            if (!_enemyHealthBarDictionary.ContainsKey(enemy))
            {
                return;
            }

            var healthBar = _enemyHealthBarDictionary[enemy];
            healthBar.SetHealthValue(health);
        }

        public void SetRoot(Enemy.Enemy enemy)
        {
            _healthBar.TryGetComponent<HealthBarPositionController>(out var healthBarPositionController);
            healthBarPositionController.SetPlayerRoot(enemy.gameObject);
        }

        public void DestroyHealthBar(Enemy.Enemy enemy)
        {
            if (!_enemyHealthBarDictionary.ContainsKey(enemy))
            {
                return;
            }

            _enemyHealthBarDictionary.Remove(enemy, out var healthBar);
            healthBar.ReleaseHealthBar();
        }

        public void DestroyAllHealthBar()
        {
            _copyEnemyHealthBarDictionary = _enemyHealthBarDictionary;
            foreach (var keyValuePair in _copyEnemyHealthBarDictionary)
            {
                var healthBar = _copyEnemyHealthBarDictionary[keyValuePair.Key];
                healthBar.Destroy();
            }

            _enemyHealthBarDictionary.Clear();
            _copyEnemyHealthBarDictionary.Clear();
            _healthBarObjectPool.Clear();
        }

        private void OnDestroyAction(HealthBar healthBar)
        {
            healthBar.ReleaseHealthBarAction -= ReleaseHealthBar;
            healthBar.Destroy();
        }

        private void ReleaseHealthBar(HealthBar healthBar)
        {
            _healthBarObjectPool.Release(healthBar);
        }

        private void OnRealise(HealthBar healthBar)
        {
            healthBar.Initialize(_maxHealth);
            healthBar.gameObject.SetActive(false);
        }

        private void OnTake(HealthBar healthBar)
        {
            healthBar.gameObject.SetActive(true);
        }

        private HealthBar OnHealthBarCreate()
        {
            var healthBar = _healthBarFactory.Create(_healthBarPrefab, _healthBarParent);
            healthBar.Initialize(_maxHealth);
            healthBar.ReleaseHealthBarAction += ReleaseHealthBar;
            return healthBar;
        }
    }
}
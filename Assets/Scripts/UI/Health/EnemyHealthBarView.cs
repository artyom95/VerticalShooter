using System.Collections.Generic;
using Settings;
using UnityEngine;

namespace UI.Health
{
    public class EnemyHealthBarView
    {
        private Dictionary<Enemy.Enemy, HealthBar> _enemyHealthBarDictionary = new();
        private Dictionary<Enemy.Enemy, HealthBar> _copyEnemyHealthBarDictionary = new ();


        private HealthBar _healthBar;
        private float _maxHealth;
        private HealthBar _healthBarPrefab;
        private HealthBarSpawner _healthBarSpawner;
       
        public void Initialize(GameSettings gameSettings,
            HealthBar healthBarPrefab,
            HealthBarSpawner healthBarSpawner)
        {
            _healthBarSpawner = healthBarSpawner;
            _healthBarPrefab = healthBarPrefab;
            _maxHealth = gameSettings.AmountEnemyHealth;
        }

        public void CreateHealthBar(Enemy.Enemy enemy, Transform parent)
        {
            _healthBar = _healthBarSpawner.SpawnHealthBar(_healthBarPrefab, parent);
            _healthBar.Initialize(_maxHealth);
            
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
            healthBar.Destroy();
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
        }
    }
}
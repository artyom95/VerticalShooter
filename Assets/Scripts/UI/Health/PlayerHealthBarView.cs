using Settings;
using UnityEngine;

namespace UI.Health
{
    public class PlayerHealthBarView 

    {
        private HealthBar _healthBar;
        private float _maxHealth;
        private HealthBar _healthBarPrefab;
        private HealthBarSpawner _healthBarSpawner;

        public void  Initialize(GameSettings gameSettings,
            HealthBar healthBarPrefab, 
            HealthBarSpawner healthBarSpawner)
        {
            _healthBarSpawner = healthBarSpawner;
            _healthBarPrefab = healthBarPrefab;
            _maxHealth = gameSettings.PlayerHealth;
        }

        public void CreateHealthBar(Transform parent)
        {
            _healthBar = _healthBarSpawner.SpawnHealthBar(_healthBarPrefab, parent);
            _healthBar.Initialize(_maxHealth);
        }
        public void ShowHealth(float health)
        {
            _healthBar.SetHealthValue(health);
        }

        public void SetRoot(Player.Player player)
        {
            _healthBar.TryGetComponent<HealthBarPositionController>(out var healthBarPositionController);
            healthBarPositionController.SetPlayerRoot(player.gameObject);
        }

        public void DestroyHealthBar()
        {
            _healthBar.Destroy();
        }
    }
}
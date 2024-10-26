using UnityEngine;

namespace UI.Health
{
    public class HealthBarSpawner
    {
        public HealthBar SpawnHealthBar(HealthBar healthBarPrefab, Transform parent)
        {
            var healthBar = Object.Instantiate(healthBarPrefab, parent);
            return healthBar;
        }
    }
}
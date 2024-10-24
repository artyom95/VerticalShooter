using UnityEngine;

namespace Enemy
{
    public class EnemySpawner
    {
        public Enemy SpawnEnemy(Enemy enemyPrefab, Transform spawnPosition)
        {
            var enemyInstance = Object.Instantiate(enemyPrefab, spawnPosition.position, Quaternion.identity);
            enemyInstance.transform.Rotate(0,0,180);
            return enemyInstance;
        }
    }
}
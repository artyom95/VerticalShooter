using AbstractClasses;
using UnityEngine;

namespace Enemy
{
    public class EnemyFactory : Factory
    {
        public override T Create<T>(T prefab, Transform parent)
        {
            var enemyInstance = Object.Instantiate(prefab, parent.position, Quaternion.identity);
            return enemyInstance;
        }
    }
}
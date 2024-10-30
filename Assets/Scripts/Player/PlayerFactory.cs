using AbstractClasses;
using UnityEngine;

namespace Player
{
    public class PlayerFactory : Factory
    {
        public override T Create<T>(T prefab, Transform parent)
        {
            var playerInstance = Object.Instantiate(prefab, parent.position, Quaternion.identity);
            return playerInstance;
        }
    }
}
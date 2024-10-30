using AbstractClasses;
using UnityEngine;

namespace UI.Health
{
    public class HealthBarFactory : Factory
    {
        public override T Create<T>(T prefab, Transform parent)
        {
            var gameObject = Object.Instantiate(prefab, parent);
            return gameObject;
        }
    }
}
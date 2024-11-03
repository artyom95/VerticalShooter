using AbstractClasses;
using UnityEngine;

namespace UI.Health
{
    public class HealthBarFactory : Factory
    {
        public override T Create<T>(T prefab, Transform parent = null)
        {
            T gameObject;
            if (parent == null)
            {
                 gameObject = Object.Instantiate(prefab, parent); 
            }
            else
            {
                 gameObject = Object.Instantiate(prefab, parent);

            }
            return gameObject;
        }
    }
}
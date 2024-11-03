using UnityEngine;

namespace AbstractClasses
{
    public abstract class Factory
    {
        public abstract T Create<T>(T prefab, Transform parent = null) where T : Object;
    }
}
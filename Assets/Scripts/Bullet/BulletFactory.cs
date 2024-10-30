using AbstractClasses;
using UnityEngine;

public class BulletFactory : Factory
{
    public override T Create<T>(T prefab, Transform parent)
    {
        var bullet = Object.Instantiate(prefab, parent.position, Quaternion.identity);
        return bullet;
    }
}
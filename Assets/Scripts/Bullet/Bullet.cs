using System;
using Settings;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public event Action<Bullet> ReleaseBulletAction;

    [SerializeField] private Rigidbody _rigidbody;

    private float _bulletSpeed;

    /// <summary>
    /// Debug Session
    /// </summary>
    private Enemy.Enemy _enemy;

    public void Initialize(GameSettings gameSettings)
    {
        _bulletSpeed = gameSettings.BulletSpeed;
    }

    public void Move(Enemy.Enemy targetEnemy)
    {
        _enemy = targetEnemy;
        var direction = (targetEnemy.transform.position - transform.position).normalized;
        _rigidbody.velocity = direction * _bulletSpeed;
    }

    public void ReleaseBullet()
    {
        ReleaseBulletAction?.Invoke(this);
        _enemy = null;
    }
}
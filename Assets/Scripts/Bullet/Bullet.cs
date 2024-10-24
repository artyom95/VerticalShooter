using System;
using DG.Tweening;
using Settings;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    private float _bulletSpeed;

    public void Initialize(GameSettings gameSettings)
    {
        _bulletSpeed = gameSettings.BulletSpeed;
    }

    public void Move(Transform movePosition)
    {
        var targetPosition = movePosition.position;
        var enemyPosition = new Vector2(targetPosition.x, targetPosition.y);
        var bulletPosition = new Vector2(_rigidbody.position.x, _rigidbody.position.y);
        var direction = (enemyPosition -  bulletPosition).normalized;

        // Двигаем пулю
        _rigidbody.AddForce(direction * _bulletSpeed, ForceMode.Impulse);
    }
}
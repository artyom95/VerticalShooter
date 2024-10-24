using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class FinishLineBehaviour: MonoBehaviour
{
    public event Action EnemyDestroyedAction;
    public event Action<Enemy.Enemy> EnemyDestroyed; 
        private void OnTriggerEnter(Collider someCollider)
        {
                if (someCollider== null )
                {
                       return;
                }

                if ( someCollider.TryGetComponent<Enemy.Enemy>(out var enemy))
                {
                    EnemyDestroyed?.Invoke(enemy);
                    enemy.KillEnemy();
                    EnemyDestroyedAction?.Invoke();
                }
        }
}
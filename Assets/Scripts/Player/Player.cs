using System;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        public Rigidbody RigidBody { get; private set; }

        private EnemyDetector _enemyDetector;

        // should delete it in the future;
        private float _radiusDetection = 4f;

        public void Awake()
        {
            RigidBody = gameObject.GetComponent<Rigidbody>();
        }

        public void Initialize(EnemyDetector enemyDetector)
        {
            _enemyDetector = enemyDetector;
        }

        public void Update()
        {
            _enemyDetector?.DetectEnemies(this);
        }

        public void Destroy()
        {
            if (gameObject == null)
            {
                return;
            }

            Destroy(gameObject);
        }

        private void OnDrawGizmos()
        {
            // Отображение радиуса обнаружения в редакторе
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _radiusDetection);
        }
    }
}
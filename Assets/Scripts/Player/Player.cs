using Settings;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        public Rigidbody RigidBody { get; private set; }
        private EnemyDetector _enemyDetector;

        private float _radiusDetection;

        public void Awake()
        {
            RigidBody = gameObject.GetComponent<Rigidbody>();
        }

        public void Initialize(EnemyDetector enemyDetector,
            GameSettings gameSettings)
        {
            _enemyDetector = enemyDetector;
            _radiusDetection = gameSettings.RadiusDetection;
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
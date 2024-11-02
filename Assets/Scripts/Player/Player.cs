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
        private float _nearRadiusDetection;

        public void Awake()
        {
            RigidBody = gameObject.GetComponent<Rigidbody>();
        }

        public void Initialize(EnemyDetector enemyDetector,
            GameSettings gameSettings)
        {
            _enemyDetector = enemyDetector;
            _radiusDetection = gameSettings.RadiusDetection;
            _nearRadiusDetection = gameSettings.NearDistanceToEnemy;
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

        /// <summary>
        /// only for Debug Session
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _radiusDetection);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _nearRadiusDetection);
        }
    }
}
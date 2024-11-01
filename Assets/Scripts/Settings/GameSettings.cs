using System;
using UnityEngine;

namespace Settings
{
    [Serializable]
    [CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings", order = 1)]
    public class GameSettings : ScriptableObject

    {
        [field: SerializeField] public TimerSettings TimerSettings;

        [field: SerializeField] public float PlayerHealth;
        [field: SerializeField] public float PlayerMoveDuration;
        [field: SerializeField] public float PlayerSpeed;
        [field: SerializeField] public float BulletSpeed;
        [field: SerializeField] public float EnemySpeed;
        [field: SerializeField] public float RadiusDetection;
        [field: SerializeField] public float NearDistanceToEnemy;

        [field: SerializeField] public float SpawnTimer;
        [field: SerializeField] public int AmountEnemy;
        [field: SerializeField] public float EnemyHealth;
        [field: SerializeField] public LayerMask EnemyLayer;
    }
}
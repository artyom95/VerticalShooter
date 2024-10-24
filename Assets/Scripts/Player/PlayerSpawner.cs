using UnityEngine;

namespace Player
{
    public class PlayerSpawner
    {
        public Player SpawnPlayer(Player playerPrefab, Transform spawnPosition)
        {
            var playerInstance = Object.Instantiate(playerPrefab, spawnPosition.position, Quaternion.identity);
            return playerInstance;
        }
    }
}
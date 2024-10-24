using UnityEngine;

    public class BulletSpawner
    {
        public Bullet CreateBullet(Bullet bulletPrefab,Transform spawnPosition)
        {
            var bullet = Object.Instantiate(bulletPrefab, spawnPosition.position, Quaternion.identity);
            return bullet;
        }
    }

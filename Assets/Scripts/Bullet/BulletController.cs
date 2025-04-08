using Settings;
using UnityEngine;
using UnityEngine.Pool;

public class BulletController
{
    private readonly Bullet _bulletPrefab;
    private ObjectPool<Bullet> _bulletPool;
    private BulletFactory _bulletFactory;
    private Player.Player _player;
    private GameSettings _gameSettings;

    public BulletController(Bullet bulletPrefab)
    {
        _bulletPrefab = bulletPrefab;
    }

    public void Initialize(Player.Player player, GameSettings gameSettings)
    {
        _gameSettings = gameSettings;
        _player = player;
        _bulletPool = new ObjectPool<Bullet>(OnBulletCreate, OnTake,
            OnRelease, OnDestroyAction,
            true, 2,
            10);
        _bulletFactory = new BulletFactory();
    }

    public void ShootBullet(Enemy.Enemy enemy)
    {
        var bullet = CreateBullet();
        bullet.Move(enemy);
    }
    private Bullet CreateBullet()
    {
        var bullet = _bulletPool.Get();
        return bullet;
    }

    private void OnDestroyAction(Bullet bullet)
    {
        bullet.ReleaseBulletAction -= ReleaseBullet;
    }

    private void OnRelease(Bullet bullet)
    {
        var transformBullet = bullet.transform;

        bullet.gameObject.SetActive(false);
        transformBullet.rotation = Quaternion.identity;
        transformBullet.SetParent(_player.transform);
        transformBullet.position = _player.transform.position;
    }

    private void OnTake(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    private Bullet OnBulletCreate()
    {
        var bullet = _bulletFactory.Create(_bulletPrefab, _player.transform);
        bullet.Initialize(_gameSettings);
        bullet.ReleaseBulletAction += ReleaseBullet;
        return bullet;
    }

    private void ReleaseBullet(Bullet bullet)
    {
        _bulletPool.Release(bullet);
    }
}
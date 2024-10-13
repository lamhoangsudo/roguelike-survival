using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform offset;
    private ObjectPool<GameObject> projectilePool;
    private HashSet<GameObject> activeProjectiles = new HashSet<GameObject>();

    private void Awake()
    {
        projectilePool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(projectile, offset.position, offset.rotation),
            actionOnGet: (projectile) => projectile.SetActive(true),
            actionOnRelease: (projectile) => projectile.SetActive(false),
            actionOnDestroy: (projectile) => Destroy(projectile),
            collectionCheck: true,
            defaultCapacity: 5,
            maxSize: 10
        );
    }

    private void Start()
    {
        GameInputSystem.instance.OnPlayerShooter += GameInputSystem_OnPlayerShooter;
        ProjectileHit.OnAnyProjectileHit += ProjectileHit_OnAnyProjectileHit;
        Projectile.OnAnyProjectileRunOutExistenceTime += Projectile_OnAnyProjectileRunOutExistenceTime;
    }

    private void Projectile_OnAnyProjectileRunOutExistenceTime(object sender, GameObject projectile)
    {
        if (activeProjectiles.Contains(projectile))
        {
            projectilePool.Release(projectile);
            activeProjectiles.Remove(projectile);
        }
    }

    private void ProjectileHit_OnAnyProjectileHit(object sender, GameObject projectile)
    {
        if (activeProjectiles.Contains(projectile))
        {
            projectilePool.Release(projectile);
            activeProjectiles.Remove(projectile);
            SoundManager.Instance.PlaySound(SoundManager.Sound.GunHit);
        }
    }

    private void GameInputSystem_OnPlayerShooter(object sender, EventArgs e)
    {
        if (GameManager.Instance.status == GameManager.GameStatus.Pause || GameManager.Instance.status == GameManager.GameStatus.GameOver) return;
        GameObject projectile = projectilePool.Get();
        activeProjectiles.Add(projectile);
        ProjectileMovement projectileMovement = projectile.GetComponent<ProjectileMovement>();
        projectileMovement.SetProjectileMovement(offset.up, offset.position, offset.rotation);
        SoundManager.Instance.PlaySound(SoundManager.Sound.GunShoot);
    }
}

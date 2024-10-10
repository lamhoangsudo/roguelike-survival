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
    private void Awake()
    {
        projectilePool = new(
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
        projectilePool.Release(projectile);
    }

    private void ProjectileHit_OnAnyProjectileHit(object sender, GameObject projectile)
    {
        projectilePool.Release(projectile);
    }

    private void GameInputSystem_OnPlayerShooter(object sender, EventArgs e)
    {
        ProjectileMovement projectileMovement = projectilePool.Get().GetComponent<ProjectileMovement>();
        projectileMovement.SetProjectileMovement(offset.up, offset.position, offset.rotation);
    }
}

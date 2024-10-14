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
    private HashSet<GameObject> activeProjectiles;
    [SerializeField] private Status status;
    [SerializeField] private int magazineMax;
    [SerializeField] private int magazine;
    [SerializeField] private float timeRateOfFireMax;
    [SerializeField] private float timeRateOfFire;
    [SerializeField] private float timeReloadMax;
    [SerializeField] private float timeReload;
    private enum Status
    {
        Ready,
        Shoot,
        Reload,
    }
    private void Awake()
    {
        activeProjectiles = new();
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
        status = Status.Ready;
        magazine = magazineMax;
        ProjectileHit.OnAnyProjectileHit += ProjectileHit_OnAnyProjectileHit;
        Projectile.OnAnyProjectileRunOutExistenceTime += Projectile_OnAnyProjectileRunOutExistenceTime;
    }
    private void Update()
    {
        Shoot();
        HandlerGunStatus();
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
    private void Shoot()
    {
        if (GameManager.Instance.status == GameManager.GameStatus.Pause || GameManager.Instance.status == GameManager.GameStatus.GameOver) return;
        if (status != Status.Ready) return;
        if (!GameInputSystem.instance.isShoot) return;
        status = Status.Shoot;
        timeRateOfFire = timeRateOfFireMax;
        magazine -= 1;
        GameObject projectile = projectilePool.Get();
        activeProjectiles.Add(projectile);
        ProjectileMovement projectileMovement = projectile.GetComponent<ProjectileMovement>();
        projectileMovement.SetProjectileMovement(offset.up, offset.position, offset.rotation);
        SoundManager.Instance.PlaySound(SoundManager.Sound.GunShoot);
    }
    private void HandlerGunStatus()
    {
        switch(status)
        {
            case Status.Ready:
                break;
                case Status.Shoot:
                timeRateOfFire -= Time.deltaTime;
                if (timeRateOfFire > 0) return;
                if(magazine <= 0)
                {
                    timeReload = timeReloadMax;
                    status = Status.Reload;
                }
                else
                {
                    status = Status.Ready;
                }
                break;
            case Status.Reload:
                timeReload -= Time.deltaTime;
                if (timeReload > 0) return;
                magazine = magazineMax;
                status = Status.Ready;
                break;
        }
    }
}

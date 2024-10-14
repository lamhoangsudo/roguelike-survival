using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using static Cinemachine.DocumentationSortingAttribute;

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
    [Range(0, 1)]
    [SerializeField] private float reduceMutiplyTimeRateOfFireMax;
    [SerializeField] private float timeRateOfFire;
    [SerializeField] private float timeReloadMax;
    [Range(0, 1)]
    [SerializeField] private float reduceMutiplyTimeReloadMax;
    [SerializeField] private float timeReload;
    [SerializeField] private Animator reloadText;
    private float timeRateOfFireMax0;
    private float timeReloadMax0;

    private enum Status
    {
        Ready,
        Shoot,
        Reload,
    }
    private void Awake()
    {
        CalculatorPlayerShooterScale(LevelSystem.instance.level);
        activeProjectiles = new();
        projectilePool = new(
            createFunc: () => Instantiate(projectile, offset.position, offset.rotation),
            actionOnGet: (projectile) => {
                projectile.SetActive(true);
            },
            actionOnRelease: (projectile) =>
            {
                projectile.SetActive(false); 
                projectile.transform.SetPositionAndRotation(offset.position, offset.rotation);
            },
            actionOnDestroy: (projectile) => Destroy(projectile),
            collectionCheck: true,
            defaultCapacity: 5,
            maxSize: 50
        );
    }
    private void Start()
    {
        status = Status.Ready;
        magazine = magazineMax;
        ProjectileHit.OnAnyProjectileHit += ProjectileHit_OnAnyProjectileHit;
        Projectile.OnAnyProjectileRunOutExistenceTime += Projectile_OnAnyProjectileRunOutExistenceTime;
        LevelSystem.instance.OnLevelChanged += LevelSystem_OnLevelChanged;
    }

    private void LevelSystem_OnLevelChanged(object sender, EventArgs e)
    {
        CalculatorPlayerShooterScale(LevelSystem.instance.level);
        magazineMax++;
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
        timeRateOfFire = timeRateOfFireMax0;
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
                reloadText.gameObject.SetActive(false);
                break;
                case Status.Shoot:
                timeRateOfFire -= Time.deltaTime;
                if (timeRateOfFire > 0) return;
                if(magazine <= 0)
                {
                    timeReload = timeReloadMax0;
                    status = Status.Reload;
                }
                else
                {
                    status = Status.Ready;
                }
                break;
            case Status.Reload:
                reloadText.gameObject.SetActive(true);
                timeReload -= Time.deltaTime;
                if (timeReload > 0) return;
                magazine = magazineMax;
                status = Status.Ready;
                break;
        }
    }
    private void CalculatorPlayerShooterScale(int level)
    {
        timeRateOfFireMax0 -= timeRateOfFireMax - timeRateOfFireMax * reduceMutiplyTimeRateOfFireMax * level;
        timeRateOfFireMax0 = Mathf.Clamp(timeRateOfFireMax, 0.2f, timeRateOfFireMax);
        timeReloadMax0 = timeReloadMax - timeReloadMax * reduceMutiplyTimeReloadMax * level;
        timeReloadMax0 = Mathf.Clamp(timeRateOfFireMax, 1f, timeReloadMax);
    }
    private void OnDestroy()
    {
        projectilePool.Clear();
        Projectile.OnAnyProjectileRunOutExistenceTime -= Projectile_OnAnyProjectileRunOutExistenceTime;
    }
}

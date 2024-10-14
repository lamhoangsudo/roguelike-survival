using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float damage;
    [Range(0, 1)]
    [SerializeField] private float damageMutiplyByLevel;
    [SerializeField] private float ExistenceTime;
    private float damage0;

    public static event EventHandler<GameObject> OnAnyProjectileRunOutExistenceTime;
    private void Awake()
    {
        CalculatorProjectileLevelScale(LevelSystem.instance.level);
    }
    private void Start()
    {
        LevelSystem.instance.OnLevelChanged += LevelSystem_OnLevelChanged;
    }

    private void LevelSystem_OnLevelChanged(object sender, EventArgs e)
    {
        CalculatorProjectileLevelScale(LevelSystem.instance.level);
    }

    private void OnEnable()
    {
        Invoke(nameof(ProjectileRunOutExistenceTime), ExistenceTime);
    }
    private void OnDisable()
    {
        CancelInvoke();
    }
    private void OnDestroy()
    {
        CancelInvoke();
    }
    public float GetDamage()
    {
        return damage0;
    }
    private void ProjectileRunOutExistenceTime()
    {
        OnAnyProjectileRunOutExistenceTime?.Invoke(this, this.gameObject);
    }
    private void CalculatorProjectileLevelScale(int level)
    {
        damage0 = damage + damage * damageMutiplyByLevel * level;
    }
}

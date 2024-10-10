using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float ExistenceTime;
    public static event EventHandler<GameObject> OnAnyProjectileRunOutExistenceTime;
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
        return damage;
    }
    private void ProjectileRunOutExistenceTime()
    {
        OnAnyProjectileRunOutExistenceTime?.Invoke(this, this.gameObject);
    }
}

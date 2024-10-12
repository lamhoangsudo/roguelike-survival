using System;
using UnityEngine;

public class ProjectileHit : MonoBehaviour
{
    [SerializeField] private int hitLayer;
    public static event EventHandler<GameObject> OnAnyProjectileHit;
    public void OnCollisionEnter2D (Collision2D collision)
    {
        if(collision.gameObject.layer == hitLayer)
        {
            OnAnyProjectileHit?.Invoke(this, this.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbodyProjectile;
    private Vector3 moveDirection;
    [SerializeField] private float ProjectileSpeed;
    public void SetProjectileMovement(Vector3 moveDirection, Vector3 shootPosition, Quaternion shootRotation)
    {
        transform.position = shootPosition;
        transform.rotation = shootRotation;
        this.moveDirection = moveDirection;
        rigidbodyProjectile.velocity = moveDirection * ProjectileSpeed;

    }
}

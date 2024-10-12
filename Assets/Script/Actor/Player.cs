using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float heath;
    [SerializeField] private float heathMax;
    [SerializeField] private float layer;
    [SerializeField] private Vector3 playerPosition;
    public static event EventHandler OnPlayerDie;
    private void Start()
    {
        heath = heathMax;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == layer)
        {
            PlayerHit(collision.gameObject.GetComponent<Enemy>().GetDamage());
        }
    }
    private void PlayerHit(float damage)
    {
        heath -= damage;
        if(heath <= 0)
        {
            OnPlayerDie?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
        }
    }
    public Vector3 GetPlayerPosition()
    {
        playerPosition = transform.position;
        return transform.position;
    }
}

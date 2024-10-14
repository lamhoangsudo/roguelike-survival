using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float heath;
    [SerializeField] private float heathMax;
    [Range(0, 1)]
    [SerializeField] private float heathMaxMutiplyByLevel;
    [SerializeField] private float reduceDamage;
    [Range(0, 1)]
    [SerializeField] private float reduceDamageMutiplyByLevel;
    [SerializeField] private float layer;
    [SerializeField] private Vector3 playerPosition;
    public static event EventHandler OnPlayerDie;
    private void Start()
    {
        heath = heathMax;
        CalculatorPlayerLevelScale(LevelSystem.instance.level);
        LevelSystem.instance.OnLevelChanged += LevelSystem_OnLevelChanged;
    }

    private void LevelSystem_OnLevelChanged(object sender, EventArgs e)
    {
        CalculatorPlayerLevelScale(LevelSystem.instance.level);
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
        damage -= reduceDamage;
        heath -= damage;
        if(heath <= 0)
        {
            OnPlayerDie?.Invoke(this, EventArgs.Empty);
            CameraShake.Instance.setShake(10f, 0.2f);
            Destroy(gameObject);
        }
        else
        {
            CameraShake.Instance.setShake(7f, 0.15f);
        }
    }
    public Vector3 GetPlayerPosition()
    {
        playerPosition = transform.position;
        return transform.position;
    }
    private void CalculatorPlayerLevelScale(int level)
    {
        heathMax += heathMax * heathMaxMutiplyByLevel * level;
        reduceDamage += reduceDamage * reduceDamageMutiplyByLevel * level;
    }
}

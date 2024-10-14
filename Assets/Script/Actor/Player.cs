using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float heath { get; private set; }
    [SerializeField] private float heathMax;
    [Range(0, 1)]
    [SerializeField] private float heathMaxMutiplyByLevel;
    [SerializeField] private float reduceDamage;
    [Range(0, 1)]
    [SerializeField] private float reduceDamageMutiplyByLevel;
    [SerializeField] private float layer;
    public static event EventHandler OnPlayerDie;
    public event EventHandler OnPlayerDamage;
    private float heathMax0;
    private float reduceDamage0;
    private void Awake()
    {
        CalculatorPlayerLevelScale(LevelSystem.instance.level);
        heath = heathMax0;
    }
    private void Start()
    {
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
        damage -= reduceDamage0;
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
            OnPlayerDamage?.Invoke(this, EventArgs.Empty);
        }
    }
    public Vector3 GetPlayerPosition()
    {
        return transform.position;
    }
    private void CalculatorPlayerLevelScale(int level)
    {
        heathMax0 = heathMax + heathMax * heathMaxMutiplyByLevel * level;
        reduceDamage0 = reduceDamage + reduceDamage * reduceDamageMutiplyByLevel * level;
    }
    public float GetHeathMax()
    {
        return heathMax0;
    }
    public float GetHeathNormalize()
    {
        return heath / heathMax0;
    }
}

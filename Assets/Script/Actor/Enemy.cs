using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float heathMax;
    [SerializeField] private float damage;
    [SerializeField] private List<int> hitLayer;
    [SerializeField] private float heath;
    [SerializeField] private float scorePoint;
    [SerializeField] private float experiencePoints;
    [Range(0, 1)]
    [SerializeField] private float scorePointsMultipleByLevel;
    [Range(0, 1)]
    [SerializeField] private float experiencePointsMultipleByLevel;
    [Range(0, 1)]
    [SerializeField] private float damagePointsMultipleByLevel;
    [Range(0, 1)]
    [SerializeField] private float heathPointsMultipleByLevel;
    private float scorePoint0;
    private float experiencePoints0;
    private float damage0;
    private float heathMax0;
    public static event EventHandler<GameObject> OnAnyEnemyDie;
    public event EventHandler OnEnemyDamage;
    public void ResetEnemy()
    {
        CalculatorEnemyLevelScale(LevelSystem.instance.level);
        heath = heathMax0;
    }
    private void Awake()
    {
        CalculatorEnemyLevelScale(LevelSystem.instance.level);
    }
    private void Start()
    {
        LevelSystem.instance.OnLevelChanged += LevelSystem_OnLevelChanged;
    }

    private void LevelSystem_OnLevelChanged(object sender, EventArgs e)
    {
        CalculatorEnemyLevelScale(LevelSystem.instance.level);
    }
    private void EnemyHit(float projectileDamage)
    {
        heath -= projectileDamage;
        if (heath <= 0)
        {
            SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyExplosion);
            CameraShake.Instance.setShake(6f, 0.15f);
            OnAnyEnemyDie?.Invoke(this, this.gameObject);
        }
        else
        {
            CameraShake.Instance.setShake(5f, 0.1f);
            OnEnemyDamage?.Invoke(this, EventArgs.Empty);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == hitLayer[0])
        {
            OnAnyEnemyDie?.Invoke(this, this.gameObject);
        }
        if(collision.gameObject.layer == hitLayer[1])
        {
            EnemyHit(collision.gameObject.GetComponent<Projectile>().GetDamage());
            CameraShake.Instance.setShake(5f, 0.1f);
        }
    }
    public float GetDamage() { return damage0; }
    public float GetScorePoint() { return scorePoint0; }
    public float GetExperiencePoints() { return experiencePoints0; }
    private void CalculatorEnemyLevelScale(int level)
    {
        scorePoint0 = scorePoint + scorePointsMultipleByLevel * scorePoint * level;
        experiencePoints0 = experiencePoints + experiencePointsMultipleByLevel * experiencePoints * level;
        damage0 = damage + damagePointsMultipleByLevel * damage * level;
        heathMax0 = heathMax + heathPointsMultipleByLevel * heathMax * level;
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

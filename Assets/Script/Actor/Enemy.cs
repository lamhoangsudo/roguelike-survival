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
    //[SerializeField] private float money;
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
    public static event EventHandler<GameObject> OnAnyEnemyDie;
    public void ResetEnemy()
    {
        heath = heathMax;
        CalculatorEnemyLevelScale(LevelSystem.instance.level);
    }
    private void Start()
    {
        heath = heathMax;
        CalculatorEnemyLevelScale(LevelSystem.instance.level);
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
    //public float GetMoney() { return money; }
    public float GetDamage() { return damage; }
    public float GetScorePoint() { return scorePoint; }
    public float GetExperiencePoints() { return experiencePoints; }
    private void CalculatorEnemyLevelScale(int level)
    {
        scorePoint += scorePointsMultipleByLevel * scorePoint * level;
        experiencePoints += experiencePointsMultipleByLevel * experiencePoints * level;
        damage += damagePointsMultipleByLevel * damage * level;
        heathMax += heathPointsMultipleByLevel * heathMax * level;
    }
}

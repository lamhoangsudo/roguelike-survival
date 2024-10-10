using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    [SerializeField] private int level;
    [SerializeField] private int levelMax;
    [SerializeField] private float experiencePoints;
    [SerializeField] private float experiencePointsMax;
    [Range(0, 1)]
    [SerializeField] private float experiencePointsMultipleByLevel;
    public static LevelSystem instance;
    public event EventHandler<int> OnLevelChanged;
    private void Awake()
    {
        if (instance == null) instance = this; 
    }
    private void Start()
    {
        Enemy.OnAnyEnemyDie += Enemy_OnAnyEnemyDie;
        CaculaterexperiencePointsMax();
    }

    private void Enemy_OnAnyEnemyDie(object sender, GameObject enemy)
    {
        if (level < levelMax)
        {
            CaculaterLevel(enemy.GetComponent<Enemy>().GetExperiencePoints());
        }
    }
    private void CaculaterexperiencePointsMax()
    {
        experiencePointsMax += experiencePointsMultipleByLevel * experiencePointsMax * level;
    }
    private void CaculaterLevel(float enemyExperiencePoints)
    {
        experiencePoints += enemyExperiencePoints;
        if(experiencePoints >= experiencePointsMax)
        {
            level += 1;
            CaculaterexperiencePointsMax();
            experiencePoints = 0;
            OnLevelChanged?.Invoke(this, level);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSystem : MonoBehaviour
{
    [SerializeField] private float playerPoint;
    public static PointSystem instance;
    public event EventHandler<float> OnPlayerPointChange;
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    private void Start()
    {
        Enemy.OnAnyEnemyDie += Enemy_OnAnyEnemyDie;
    }

    private void Enemy_OnAnyEnemyDie(object sender, GameObject enemyDead)
    {
        playerPoint += enemyDead.GetComponent<Enemy>().GetScorePoint();
        OnPlayerPointChange?.Invoke(this, playerPoint);
    }
}

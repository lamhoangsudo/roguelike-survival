using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour, IMovement
{
    public Player player;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D enemyRigidbody;
    private bool isFullWaveEnemyReady;
    private void Start()
    {
        isFullWaveEnemyReady = false;
    }
    private void Update()
    {
        if (player == null && isFullWaveEnemyReady) return;
        Move(player.GetPlayerPosition());
    }
    public void Move(Vector3 movePosition)
    {
        Vector3 moveDir = (movePosition - this.transform.position).normalized;
        GetComponent<Rigidbody2D>().velocity = moveSpeed * moveDir;
        EnemyWaveManager.instance.OnFullWaveEnemyReady += EnemyWaveManager_OnFullWaveEnemyReady;
    }

    private void EnemyWaveManager_OnFullWaveEnemyReady(object sender, System.EventArgs e)
    {
        isFullWaveEnemyReady = true;
    }
}
